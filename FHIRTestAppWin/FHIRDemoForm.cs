using System;
using System.Configuration;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System.IO;

namespace FHIRTestAppWin
{
    public partial class fhirDemoForm : Form
    {
        private string[] _defaultScopes;

        public fhirDemoForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.resourceSelectorComboBox.SelectedIndex = 0;
            this._defaultScopes = new string[] { ConfigurationManager.AppSettings["APIDefaultScope"] };
        }



        /// <summary>
        /// Display the result of the Web API call
        /// </summary>
        /// <param name="result">Object to display</param>
        private void Display(JObject result)
        {
            this.dataTextBox.Text = result.ToString();
        }

        private async void UploadDataButton_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                fileDialog.FilterIndex = 1;
                fileDialog.RestoreDirectory = true;
                fileDialog.Multiselect = true;
                fileDialog.Title = "Patient Data Browser";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    var result = await this.GetTokenForConfidentialClient();

                    foreach (var file in fileDialog.FileNames)
                    {
                        this.statusTextBox.Text += "Start Processing: " + file + Environment.NewLine;
                        var resourceList = this.ExtractResourcesFromPatientBundle(file);

                        this.SubmitResourcesToAPI(resourceList, result.AccessToken);
                    }
                }
            }
           
        }

        /// <summary>
        /// Method to submit a list of FHIR resources to the API
        /// </summary>
        /// <param name="fhirResources"></param>
        private async void SubmitResourcesToAPI(IList<FHIRResource> fhirResources, string token)
        {
            // Acquire Token
            //var result = await this.GetTokenForConfidentialClient();

            if (!String.IsNullOrEmpty(token))
            {
                // Iterate through each resource to submit to the API
                foreach (var fhirResource in fhirResources)
                {
                    this.statusTextBox.Text += "Getting ready to invoke the API" + Environment.NewLine;
                    var httpClient = new HttpClient();
                    var apiCaller = new ProtectedApiCallHelper(httpClient);
                    await apiCaller.SendFHIRResourceDataToAPI(ConfigurationManager.AppSettings["FHIRBaseAPIURI"], token, fhirResource, Display);
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        /// This method extracts all the resources from a Synthea generated patient FHIR bundle given a file path
        /// </summary>
        /// <param name="filePath">path to the Patient Bundle JSON file</param>
        /// <returns>List of resources</returns>
        private IList<FHIRResource> ExtractResourcesFromPatientBundle(string filePath)
        {
            var fileContent = string.Empty;
            IList<FHIRResource> resourceList = new List<FHIRResource>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                fileContent = reader.ReadToEnd();
            }

            // Deserialize Patient bundle into JSON object
            JObject bundle = JObject.Parse(fileContent);
            JArray entries;
            try
            {
                // verify reference ids for all the resources
                FhirImportReferenceConverter.ConvertUUIDs(bundle);
            }
            catch
            {
                this.statusTextBox.Text += "Failed to resolve references in " + filePath + Environment.NewLine;
                return null;
            }

            try
            {
                // Get the entries from the FHIR bundle
                entries = (JArray)bundle["entry"];
                if (entries == null)
                {
                    this.statusTextBox.Text += "Failed to load file " + filePath + Environment.NewLine;
                }

                // Create a list of FHIR resources from Patient bundle
                for (int i = 0; i < entries.Count; i++)
                {
                    resourceList.Add(new FHIRResource()
                    {
                        Resource = ((JObject)entries[i])["resource"].ToString(),
                        ResourceID = (string)((JObject)entries[i])["resource"]["id"],
                        ResourceType = (string)((JObject)entries[i])["resource"]["resourceType"]
                    });
                }
            }
            catch (Exception ex)
            {
                this.statusTextBox.Text += "Failed to load file " + filePath + Environment.NewLine;
                this.statusTextBox.Text += "Error: " + ex.StackTrace + Environment.NewLine;
            }
            return resourceList;
        }

        private async void GetDataButton_Click(object sender, EventArgs e)
        {
            if (this.resourceSelectorComboBox.SelectedIndex == 0)
            {
                if (MessageBox.Show("You must select a Resource...", "Alert!!", MessageBoxButtons.OK) == DialogResult.OK)
                    return;
            }
            string uri = ConfigurationManager.AppSettings["FHIRBaseAPIURI"];

            // Ensure an ID is selected for the appropriate Resource
            if (String.IsNullOrEmpty(this.resourceIDTextBox.Text))
            {
                if (this.resourceSelectorComboBox.SelectedIndex == 1)
                {
                    if (MessageBox.Show("Please enter a Patient ID. ", "Alert!!", MessageBoxButtons.OK) == DialogResult.OK)
                        return;
                }
                else if (this.resourceSelectorComboBox.SelectedIndex == 2)
                {
                    if (MessageBox.Show("Please enter an Encounter ID. ", "Alert!!", MessageBoxButtons.OK) == DialogResult.OK)
                        return;
                }
                else
                {
                    if (MessageBox.Show("Please enter an Observation ID. ", "Alert!!", MessageBoxButtons.OK) == DialogResult.OK)
                        return;
                }
            }
            else
            {
                // Set proper URI based on the resource
                uri += @"/" + this.resourceSelectorComboBox.Text + @"/" + this.resourceIDTextBox.Text;
            }


            var result = await this.GetTokenForConfidentialClient();

            if (result != null)
            {
                this.statusTextBox.Text += "Getting ready to invoke the API" + Environment.NewLine;
                var httpClient = new HttpClient();
                var apiCaller = new ProtectedApiCallHelper(httpClient);
                //uri = String.IsNullOrEmpty(this.resourceIDTextBox.Text) ? this._patientURI : this._patientURI + @"/" + this.resourceIDTextBox.Text;
                await apiCaller.GetDataFromAPIAsync(uri, result.AccessToken, Display);
            }
        }

        private async void PostPatientData(string fileName)
        {

            using (StreamReader reader = new StreamReader(fileName))
            {
                this.statusTextBox.Text = "Reading patient data from the file..." + Environment.NewLine;
                var fileContent = reader.ReadToEnd();

                var result = await this.GetTokenForConfidentialClient();

                if (result != null)
                {
                    this.statusTextBox.Text += "Getting ready to post data to the API" + Environment.NewLine;
                    var httpClient = new HttpClient();
                    var apiCaller = new ProtectedApiCallHelper(httpClient);
                    //var uri = String.IsNullOrEmpty(this.patientIDTextBox.Text) ? this._patientURI : this._patientURI + @"/" + this.patientIDTextBox.Text;
                    await apiCaller.PostDataToAPIAsync(ConfigurationManager.AppSettings["BundleURI"], result.AccessToken, fileContent, Display);
                }
            }
        }




        /// <summary>
        /// Method to acquire the Token from Azure AD. A confidential app leverages services principal (service account). It doesn't 
        /// prompt the user for authorization. 
        /// </summary>
        /// <returns></returns>
        private async Task<AuthenticationResult> GetTokenForConfidentialClient()
        {
            // Authority by concatenating Azure AD instance and Tenant ID
            var authority = String.Format(CultureInfo.InvariantCulture, 
                ConfigurationManager.AppSettings["AADInstance"], ConfigurationManager.AppSettings["Tenant"]);

            // Setup Confidential Client App
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(ConfigurationManager.AppSettings["ClientId"])
                .WithClientSecret(ConfigurationManager.AppSettings["Secret"])
                .WithAuthority(new Uri(authority))
                .Build();

            AuthenticationResult result = null;
            try
            {
                this.statusTextBox.Text = "Getting ready to acquire Token" + Environment.NewLine;
                // Acquire Token using default scope
                result = await app.AcquireTokenForClient(new string[] {
                        ConfigurationManager.AppSettings["APIDefaultScope"]
                    })
                    .ExecuteAsync();
                this.statusTextBox.Text += "Token acquired" + Environment.NewLine;
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be of the form "https://resourceurl/.default"
                // Mitigation: change the scope to be as expected
                this.statusTextBox.Text += "Falied to get Token" + Environment.NewLine;
                this.statusTextBox.Text += "Unsupported Scope" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                // Something really went wrong. Most likely, Azure AD is not configiured
                this.statusTextBox.Text += "Falied to get Token" + Environment.NewLine;
                this.statusTextBox.Text += ex.Message + Environment.NewLine;
            }


            return result;
        }

        /// <summary>
        /// A button to ensure that I can get API Token. By default button click event is not async. 
        /// Since we need to set the Get Token asynchronously, we need to make this method async.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetConfigentialAppTokenButton_Click(object sender, EventArgs e)
        {
            var result = await this.GetTokenForConfidentialClient();

            if (result != null)
            {
                this.dataTextBox.Text = result.AccessToken;
            }
        }

        private void ResourceSelectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
