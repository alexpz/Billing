using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientsMSSql.View
{
    public class MakeFormVisible : IPageVisible
    {


        private MainWindow window = null;

        public enum SCREEN {ALLCLIENTS, UPDATECLIENTS, NEWCLIENTS, NEWCODE, BILLING};

        public MakeFormVisible(Window window)
        {

            this.window = window as MainWindow;

        }


        public void MakePageVisible(SCREEN screenname)
        {
            if (screenname == SCREEN.NEWCLIENTS)
                CreateClientPageVisible();
            else if (screenname == SCREEN.ALLCLIENTS)
            {
                AllClients();
            }
            else if (screenname == SCREEN.UPDATECLIENTS)
            {
                UpdateClientPageVisible();
            }
            else if (screenname == SCREEN.NEWCODE)
            {
                UpdateDeleteNewCodePageVisible();
            }
            else if (screenname == SCREEN.BILLING)
            {
                BillingPageVisible();
            }


            return;
        }


        private void BillingPageVisible()
        {
            window.ScreenLBL.Content = "Billing";

            window.billingSPL.Visibility = Visibility.Visible;

            window.codesCreate.Visibility = Visibility.Collapsed;
            window.client3FieldGrid.Visibility = Visibility.Collapsed;
            window.SearchBottom.Visibility = Visibility.Collapsed;
           // window.clientCodeSPL.Visibility = Visibility.Collapsed;
            window.updatePatientInfoSPNL.Visibility = Visibility.Collapsed;
            window.createClientPNL.Visibility = Visibility.Collapsed;




        }



        private void UpdateDeleteNewCodePageVisible()
        {
            window.ScreenLBL.Content = "Create, Update or Delete Code";

            window.codesCreate.Visibility = Visibility.Visible;
            window.client3FieldGrid.Visibility = Visibility.Collapsed;
            window.SearchBottom.Visibility = Visibility.Collapsed;
            //window.clientCodeSPL.Visibility = Visibility.Collapsed;
            window.updatePatientInfoSPNL.Visibility = Visibility.Collapsed;
            window.createClientPNL.Visibility = Visibility.Collapsed;
            window.billingSPL.Visibility = Visibility.Collapsed;

        }


        private void CreateClientPageVisible()
        {
            window.ScreenLBL.Content = "New Patient";

            window.updatePatientInfoSPNL.Visibility = Visibility.Visible;
            window.createClientPNL.Visibility = Visibility.Visible;

            window.UpdateDeleteClientPNL.Visibility = Visibility.Collapsed;
            window.SearchBottom.Visibility = Visibility.Collapsed;
            window.client3FieldGrid.Visibility = Visibility.Collapsed;
            window.codesCreate.Visibility = Visibility.Collapsed;
            window.billingSPL.Visibility = Visibility.Collapsed;

        }

        private void UpdateClientPageVisible()
        {
            window.ScreenLBL.Content = "Update Patient";

            window.updatePatientInfoSPNL.Visibility = Visibility.Visible;
            window.UpdateDeleteClientPNL.Visibility = Visibility.Visible;

            window.createClientPNL.Visibility = Visibility.Collapsed;
            window.SearchBottom.Visibility = Visibility.Collapsed;
            window.client3FieldGrid.Visibility = Visibility.Collapsed;
            window.codesCreate.Visibility = Visibility.Collapsed;
            window.billingSPL.Visibility = Visibility.Collapsed;

        }





        private void AllClients()
        {
            window.ScreenLBL.Content = "Patients";

            window.client3FieldGrid.Visibility = Visibility.Visible;
            //window.clientDataGrid.Visibility = Visibility.Visible;
            window.SearchBottom.Visibility = Visibility.Visible;
            //window.clientCodeSPL.Visibility = Visibility.Visible;

            window.SearchBottom.Visibility = Visibility.Visible;

            window.codesCreate.Visibility = Visibility.Collapsed;
            window.updatePatientInfoSPNL.Visibility = Visibility.Collapsed;
            window.UpdateDeleteClientPNL.Visibility = Visibility.Collapsed;
            window.createClientPNL.Visibility = Visibility.Collapsed;
            window.billingSPL.Visibility = Visibility.Collapsed;


        }





    }
}
