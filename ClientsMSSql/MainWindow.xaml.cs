using ClientsMSSql.View;
using ClientsMSSql.ViewHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientsMSSql.Extensions;
using System.Data;
using System.Windows.Markup;

namespace ClientsMSSql
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private IPageVisible formVisible;
 
        private ClientsAppContext cac;
        private readonly DataGridHelper clientCodeRelHelper = new DataGridHelper();
        bool isInsertMode = false;
        bool isBeingEdited = false;

        public MainWindow()
        {
            cac = ClientsAppContext.GetContextInstance();
           
            InitializeComponent();
            loadgrid();
        }

        private void loadgrid()
        {
            FirstLastDofAFrid.ItemsSource = cac.GetAllClients();

            List<Client> warningClient = cac.GetLiableWarningClients().ToList();
            warningClient.AddRange(cac.GetMriWarningClients());
            WarningPatientLv.ItemsSource = warningClient;
            formVisible = new MakeFormVisible(this);
        }

 
  
        private void buttonFN_Click(object sender, RoutedEventArgs e)
        {
            SearchAllBoxes();

        }

        private void buttonLN_Click(object sender, RoutedEventArgs e)
        {
            SearchAllBoxes();
        }

          
        private void SearchAllBoxes()
        {
            string ln = SearchLN.Text;
            string fn = SearchFN.Text;
  
            IQueryable<Client> fndata = null;
            IQueryable<Client> lndata = null;
            IQueryable<Client> result  = cac.GetAllIqueryableClients();

             


            fndata = GetClientBySearch(cac.GetAllIqueryableClients(), fn, Constants.SEARCHTYPE.FIRSTNAME);

            lndata = GetClientBySearch(cac.GetAllIqueryableClients(), ln, Constants.SEARCHTYPE.LASTNAME);
            if (fndata.Count() != 0)
            {
                result = result.Intersect(fndata);
            }

            if (lndata.Count() != 0)
            {
                result = result.Intersect(lndata);
            }
            FirstLastDofAFrid.ItemsSource = result.ToList();
        }
        

        private IQueryable<Client> GetClientBySearch(IQueryable<Client>  clients, string search, Constants.SEARCHTYPE searchtype)
        {
            if (!String.IsNullOrEmpty(search))
            {
                if (searchtype == Constants.SEARCHTYPE.FIRSTNAME)
                {
                    return clients.Where(a => a.FirstName.Contains(search));
                }
                else if (searchtype == Constants.SEARCHTYPE.LASTNAME)
                {
                    return clients.Where(a => a.LastName.Contains(search));
                }
                else if (searchtype == Constants.SEARCHTYPE.DESCRIPTION)
                {
                    return clients.Where(a => a.Description.Contains(search));
                }
                else if (searchtype == Constants.SEARCHTYPE.ADDRESS)
                {
                    return clients.Where(a => a.Address.Contains(search));
                }
            }
            var s = new List<Client>();
            return s.AsQueryable();

        }

        private void newCodeBTN_Click(object sender, RoutedEventArgs e)
        {
            CreateCodeGrid.ItemsSource = cac.GetAllCodes();



            formVisible.MakePageVisible(MakeFormVisible.SCREEN.NEWCODE);
        }

 
        private void ClientsBTN_Click(object sender, RoutedEventArgs e)
        {
            formVisible.MakePageVisible(MakeFormVisible.SCREEN.ALLCLIENTS);
        }

        // clicking button to go to "Patient Info " page
        private void PatientInfoBTN_Click(object sender, RoutedEventArgs e)
        {
            Client tempclient = FirstLastDofAFrid.SelectedItem as Client;
            if (tempclient == null)
            {
                MessageBox.Show("Plesae choose one of the clients !");
                formVisible.MakePageVisible(MakeFormVisible.SCREEN.ALLCLIENTS);
                return;
            }

            updatePatientInfoGRD.DataContext = tempclient;// cac.GetClientByID(tempclient.ID);

            formVisible.MakePageVisible(MakeFormVisible.SCREEN.UPDATECLIENTS);
         }


        private void NewPatientBTN_Click(object sender, RoutedEventArgs e)
        {
            updatePatientInfoGRD.DataContext = null;
            formVisible.MakePageVisible(MakeFormVisible.SCREEN.NEWCLIENTS);

        }



        private void updateClientBTN_Click(object sender, RoutedEventArgs e)
        {
            var cl = updatePatientInfoGRD.DataContext as Client;
             
            cl.LiabilityDate = !string.IsNullOrEmpty(cl.Liability) ? (DateTime?) DateTime.Now : null;
            cac.SaveChanges();
        }

        private void deleteClientBTN_Click(object sender, RoutedEventArgs e)
        {

 
            var cl = updatePatientInfoGRD.DataContext as Client;

            var Res = MessageBox.Show("Are you sure you want to delete " + cl.FirstName  + " " + cl.LastName + " ?", "Deleting Client", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (Res == MessageBoxResult.Yes)
            {
                cac.RemoveClient(cl);
                MessageBox.Show("Client " + cl.LastName + " was successfully removed !");
                loadgrid();
                formVisible.MakePageVisible(MakeFormVisible.SCREEN.ALLCLIENTS);
            }

 

        }

        private void createClientBTN_Click(object sender, RoutedEventArgs e)
        {
             

            Client cl = new Client()
            {
                FirstName = NPFirstNameTXB.Text,
                LastName = NPLastNameTXB.Text,
                DateOfAccident = NPDateOfAccidentDTP.SelectedDate?? DateTime.Now, //  NPDateOfAccidentDTP.DisplayDate,
                CreatedTime = DateTime.Now,
                Attorney = NPAttorneyTXB.Text,
                InsuranceCompany = NPNameOfInsuranceTXB.Text,
                PropertyDamage = NPPropertyDamageTXB.Text,
                Consultation = NPConsultationTXB.Text,
                PainControl = NPPainControlTXB.Text,
                ShockWave =  NPShockWaveTXB.Text,
                Mri = NPMriDPR.SelectedDate,
                Liability = NPLiabilityTXB.Text,
                LiabilityDate = ! string.IsNullOrEmpty(NPLiabilityTXB.Text) ?  DateTime.Now: default(DateTime),

            };
            cac.AddClient(cl);
            loadgrid();
            formVisible.MakePageVisible(MakeFormVisible.SCREEN.ALLCLIENTS);

        }

        private void createCodeGRD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            var code = CreateCodeGrid.SelectedItem as Code;
            if (code == null)
                return;
 
        }

        private void createCodeGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            isInsertMode = true;
             

        }

        private void createCodeGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var code = e.Row.DataContext as Code;
            if (isInsertMode)
            {
                var insertRecord  = MessageBox.Show("Do you want to add " + code.Code1 + " as a new code ?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (insertRecord == MessageBoxResult.Yes)
                {
                    code.UpdatedTime = DateTime.Now;
                    cac.AddCode(code);
                     
                    MessageBox.Show(code.Code1 + " " + code.Description + " has being added!", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                 isInsertMode = false;
            }
            else
            {
                cac.SaveChanges();
            }
        }

        private void createCodeGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            isBeingEdited = true;
        }

        private void createCodeGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Delete && !isBeingEdited)
            {
                var grid = (DataGrid)sender;
                if (grid.SelectedItems.Count > 0)
                {
                    var Res = MessageBox.Show("Are you sure you want to delete " + grid.SelectedItems.Count + " Codes ?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (Res == MessageBoxResult.Yes)
                    {
                        foreach (var row in grid.SelectedItems)
                        {
                            Code code = row as Code;
                            cac.RemoveCode(code);
                         }
                         MessageBox.Show(grid.SelectedItems.Count + " Codes have being deleted!");
                    }
                 }
             }
            isBeingEdited = false;
        }

        private void BillingBTN_Click(object sender, RoutedEventArgs e)
        {
            Client tempclient = FirstLastDofAFrid.SelectedItem as Client;
            if (tempclient == null)
            {
                MessageBox.Show("Plesae choose one of the clients !");
                formVisible.MakePageVisible(MakeFormVisible.SCREEN.ALLCLIENTS);
                return;
            }
            BillingGRD.DataContext = null;
            BillingGRD.DataContext = tempclient;
            var CCRelations = cac.GetClientOcdeRelationsByClientId(tempclient.ID);
            clientCodesRelationGRD.ItemsSource = CCRelations.ToList();



            BLCodeDGCB.ItemsSource = cac.GetAllCodes().Select(a => a.Description).ToList();
            formVisible.MakePageVisible(MakeFormVisible.SCREEN.BILLING);
 

        }

        private void clientCodesRelationGRD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             
            var selected = clientCodesRelationGRD.SelectedItem as ClientCodeRealtion   ;
             
        }
        private void clientCodesRelationGRD_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            clientCodeRelHelper.IsInsertMode = true;
 
        }

        private void clientCodesRelationGRD_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

            DataGrid dg = sender as DataGrid;
            ClientCodeRealtion clientCode  = dg.SelectedItem as ClientCodeRealtion;

            Client tempclient = FirstLastDofAFrid.SelectedItem as Client;
            
            if (clientCode.ClientID == null)
            {
                 clientCode.ClientID = tempclient.ID;
            }
 
            clientCodeRelHelper.Insert(clientCode, "New Code "); 
        }

        private void clientCodesRelationGRD_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //clientCodeRelHelper.IsBeingEdited = true;
        }

        private void clientCodesRelationGRD_PreviewKeyDown(object sender, KeyEventArgs e)
        {
         
            /*
            if (e.Key == Key.Delete && !isBeingEdited)
            {
                var grid = (DataGrid)sender;
                if (grid.SelectedItems.Count > 0)
                {
                    var Res = MessageBox.Show("Are you sure you want to delete " + grid.SelectedItems.Count + " Codes ?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (Res == MessageBoxResult.Yes)
                    {
                        foreach (var row in grid.SelectedItems)
                        {
                            Code code = row as Code;
                            cac.RemoveCode(code);
                        }
                        MessageBox.Show(grid.SelectedItems.Count + " Codes have being deleted!");
                    }
                }
            }
            isBeingEdited = false;
        */
        }

        private void clientCodesRelationGRD_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int description_column_index = 1;
            DataGrid dg = sender as DataGrid;

    
            if (e.Column.DisplayIndex == description_column_index)
            {

                ClientCodeRealtion ccr = dg.SelectedItem as ClientCodeRealtion;
                Code code = cac.GetAllCodes().Single(a => String.Equals(ccr.Description, a.Description));
                dg.GetCell(e.Row.GetIndex(), 2).Content = code.Code1;
                dg.GetCell(e.Row.GetIndex(), 3).Content = code.Amount;

            }
         }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog()==true)
            {
                string dofa = BLGDateOfAccidentTXB.Text;
                string fn = BLGFirstNameTXB.Text;
                string ln = BLGLastNameTXB.Text;

                // create a document
                FixedDocument document = new FixedDocument();
                document.DocumentPaginator.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);

                // create a page
                FixedPage page1 = new FixedPage();
                page1.Width = document.DocumentPaginator.PageSize.Width;
                page1.Height = document.DocumentPaginator.PageSize.Height;

                // add some text to the page
                TextBlock page1Text = new TextBlock();
                string printtext  = String.Format("{0,-30}{1,-30}{2,40}" + Environment.NewLine , "First Name", "Last Name", "Date of Accident");
                       printtext += String.Format("{0,-30}{1,-30}{2,40:d}", fn, ln, dofa);
                printtext += Environment.NewLine;
                printtext += Environment.NewLine;
                printtext += clientCodesRelationGRD.Items.PrintItemCollection();

                page1Text.Text = printtext;
                //page1Text.FontSize = 40; // 30pt text
                page1Text.Margin = new Thickness(48); // 0.5 inch margin
                page1.Children.Add(page1Text);
 
                // add the page to the document
                PageContent page1Content = new PageContent();
                ((IAddChild)page1Content).AddChild(page1);
                document.Pages.Add(page1Content);
                // and print
                pd.PrintDocument(document.DocumentPaginator, "Billing Document");
                 
            }

        }

        private void UpdateCode_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
 
            Client tempclient = FirstLastDofAFrid.SelectedItem as Client;
            ItemCollection items = clientCodesRelationGRD.Items ;
            
 
            IEnumerable<ClientCodeRealtion> ic =  clientCodesRelationGRD.ConvertGridDataDbEnumerableObject(tempclient.ID  );
            clientCodesRelationGRD.ItemsSource = ic.ToList();


        }
    }
}

