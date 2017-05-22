using ClientsMSSql.Extensions;
using ClientsMSSql.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientsMSSql.ViewHelper
{
    public class DataGridHelper
    {

        private bool insertMode = false;
        private bool beingEdited = false;

        public bool IsInsertMode
        {
            get { return insertMode; }
            set { insertMode = value; }
        }


        public bool IsBeingEdited
        {
            get { return beingEdited; }
            set { beingEdited = value; }
        }



        public void Insert<T>(T ob, string displayValue)
        {
            var cc = ob as ClientCodeRealtion;
             
            if (insertMode)
            {
                var InsertRecord = MessageBox.Show("Do you want to add " + displayValue + " ?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    cc.Insert();

                    
                    MessageBox.Show(displayValue + " has being added!", "Inserting Record", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                insertMode = false;
            }
            else
            {
                ClientsAppContext.GetContextInstance().SaveChanges();
            }

        }

        public void Remove<T>(T obj)
        {

        }
    }
    
}
