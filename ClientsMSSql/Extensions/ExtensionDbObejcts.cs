using ClientsMSSql.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClientsMSSql.Extensions
{
    public static class ExtensionDbObejcts
    {
        public static void Insert(this ClientCodeRealtion cc)
        {
            ClientsAppContext cac = ClientsAppContext.GetContextInstance();

            Code code = cac.GetAllCodes().Single(a => string.Equals(cc.Description, a.Description));
 


            ClientCode clientcode = new ClientCode()
            {
                client_ID = cc.ClientID,
                code_ID = code.ID,
                Description = cc.Description,
                CreatedTime = DateTime.Now,  
            };
            cac.AddClientCode(clientcode);
        }

        public static string PrintLine(this ClientCodeRealtion cc)
        {
            if (cc == null)
                return "";
            return String.Format("{0,-35:D}{1,-20}{2,-10}{3:N}"+ Environment.NewLine, cc.AddedClientCodeRelationDate, cc.Description, cc.Code, cc.Amount);
        }


        public static string PrintItemCollection(this ItemCollection cc)
        {
            string result = "";
            foreach(var it in cc)        
            {
                ClientCodeRealtion temp = it as ClientCodeRealtion;
                result += temp.PrintLine();
            }
            return result;
        }

        public static IEnumerable<ClientCodeRealtion> ConvertGridDataDbEnumerableObject(this DataGrid clientCodeDataGrid, int clientId )
        {
            var selectedList = new List<ClientCodeRealtion>();
            var cac = ClientsAppContext.GetContextInstance();


            ItemCollection items = clientCodeDataGrid.Items;

            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                ClientCodeRealtion temp = it as ClientCodeRealtion;
                if (temp == null) break;
             
                if (temp.ClientID == null)
                {
                    temp.ClientID = clientId;
                }
                 
                var code = cac.GetAllCodes().Single(a => string.Equals(temp.Description, a.Description));
                temp.CodeID = code.ID;
 
                var mycheckbox = clientCodeDataGrid.Columns[4].GetCellContent(it) as CheckBox;
                if (mycheckbox.IsChecked.Value)
                {
                    temp.ToDelete = true;
                }
                selectedList.Add(temp);
             }

            cac.RemoveAllClientCodes(clientId);
 

            foreach (var ccrs in selectedList)
            {
                if (!ccrs.ToDelete)
                {
                    var temmpClientCode = new ClientCode()
                    {
                        client_ID = ccrs.ClientID,
                        code_ID = ccrs.CodeID,
                        CreatedTime = ccrs.AddedClientCodeRelationDate,

                    };
                    cac.AddClientCode(temmpClientCode);
                }
            }             

            selectedList.RemoveAll(a => a.ToDelete);

 
            return selectedList;
        }
     }
}
