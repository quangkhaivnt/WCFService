using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        DataEmployeeDataContext data = new DataEmployeeDataContext();

        public bool AddCustomer(Customer cus)
        {
            try
            {
                data.Customers.InsertOnSubmit(cus);
                data.SubmitChanges();
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(int idC)
        {
            try
            {
                Customer cusToDelete = (from customer in data.Customers where customer.EmpID == idC select customer).Single();
                data.Customers.DeleteOnSubmit(cusToDelete);
                data.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Customer> GetCustomersList()
        {
            try
            {
                return (from customer in data.Customers select customer).ToList();
            }
            catch
            {
                return null;
            }
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public bool UpdateCustomer(Customer cus)
        {
            Customer customerToModify = (from customer in data.Customers where customer.EmpID == cus.EmpID select customer).Single();
            customerToModify.Age = cus.Age;
            customerToModify.Address = cus.Address;
            customerToModify.FirstName = cus.FirstName;
            customerToModify.LastName = cus.LastName;
            data.SubmitChanges();
            return true;
        }
    }
}
