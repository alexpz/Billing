using ClientsMSSql.View;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 


namespace ClientsMSSql
{
     
    public class ClientsAppContext
    {
        private ClientsEntities ce;
        private static ClientsAppContext cac = null;
        private ClientsAppContext()
        {
            ce = new ClientsEntities();
        }


        public static ClientsAppContext GetContextInstance() {
            if (cac == null)
            {
                cac = new ClientsAppContext();
            }

            return cac;

        }

        public IList<Client> GetAllClients()
        {
            return ce.Clients.ToList();
        }

        public IQueryable<Client> GetAllIqueryableClients()
        {
            return ce.Clients ;
        }

        public IEnumerable<Client> GetLiableWarningClients()
        {
            var sev = 7.0;
           
             
            return ce.Clients
                .Where(a => String.IsNullOrEmpty(a.Liability))
                .ToList()
                .Where(a => (a.DateOfAccident.AddDays(sev)).Date == DateTime.Today).Select(a=>
                {
                    a.WarningMessage = "Check Liability Patient";
                    return a;
                });

            
        }

        public IEnumerable<Client> GetMriWarningClients()
        {
            double[] sev = { 7.0F, 14.0F, 21.0F, 28.0F, 35.0F};
             
            return ce.Clients
                .Where(a => !String.IsNullOrEmpty(a.Liability))
                .Where(a => a.Mri == null)
                .ToList()
                .Where(a=> CheckIfLiabilityDateValid(a, sev))
                .Select(a=>
                {
                    a.WarningMessage = "Check Mri Patient ";
                    return a;
                });
        }


        private Func<Client, double[],  bool > CheckIfLiabilityDateValid = (a, b) =>
        {
            foreach (var wd in b)
            {
                if (((DateTime)(a.LiabilityDate)).AddDays(wd).Date == DateTime.Today)
                {
                    return true;
                }

            }
            return false;
        };

        private Func<Client, double[], bool> CheckIfDofaDateValid = (a, b) =>
        {
            foreach (var wd in b)
            {
                if (a.DateOfAccident.AddDays(wd).Date == DateTime.Today)
                {
                    return true;
                }

            }
            return false;
        };

        public IList<Code> GetAllCodes()
        {
            return ce.Codes.ToList();
        }


        public Client GetClientByID(int id)
        {
            return ce.Clients.Single(a => a.ID == id);
         }

         


        public IQueryable<Code> GetCodeByclientId(int clientid)
        {

            return ce.ClientCodes.Where(a => a.client_ID == clientid).Select(a => a.Code);
        }

        public IQueryable<ClientCodeRealtion> GetClientOcdeRelationsByClientId(int clientid)
        {
            return ce.ClientCodes.Where(a => a.client_ID == clientid).Select (a => 
                                                                               new ClientCodeRealtion()
                                                                               {
                                                                                   AddedClientCodeRelationDate = a.CreatedTime,
                                                                                   Description = a.Code.Description,
                                                                                   Code = a.Code.Code1,
                                                                                   Amount = a.Code.Amount,
                                                                                   ToDelete = false,
                                                                                   ClientID = a.client_ID, 
                                                                                   CodeID = a.code_ID,
                                                                               });
         }

        public void AddClient(Client c)
        {
            ce.Clients.Add(c);
            ce.SaveChanges();

        }


        public void RemoveClient(Client c)
        {
            ce.Clients.Remove(c);
            ce.SaveChanges();
        }

        public void  AddCode(Code c)
        {
            ce.Codes.Add(c);
            ce.SaveChanges ();
        }

        public void RemoveCode(Code c)
        {
            ce.Codes.Remove(c);
            ce.SaveChanges();
        }

        public void AddClientCode(ClientCode cc)
        {
            ce.ClientCodes.Add(cc);
            ce.SaveChanges();
        }

        public void RemoveClientCode(ClientCode cc)
        {
            ce.ClientCodes.Remove(cc);
        }

        public IQueryable<ClientCode> GetAllClientCodes(int clientId)
        {
            return ce.ClientCodes.Where(a => a.Client.ID == clientId);
        }

        public void RemoveAllClientCodes(int clientId)
        {
             ce.ClientCodes.RemoveRange(GetAllClientCodes(clientId));
            ce.SaveChanges();
        }




        public void SaveChanges()
        {
            ce.SaveChanges();
        }

    }
}
