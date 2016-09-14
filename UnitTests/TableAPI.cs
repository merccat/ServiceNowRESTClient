using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ServiceNow.TableAPI;
using ServiceNow.TableDefinitions;

// These are not complete nor genarlized enough to be really useful out of the box examples but can serve as a starting point / example.
// I do plan to greate a more generic set of unit tests soon.

namespace UnitTests
{
    [TestClass]
    public class TableAPI
    {
        [TestMethod]
        public void GetById()
        {
            TableAPIClient<sys_user> client = new TableAPIClient<sys_user>("sys_user", "yourInstance", "user", "pass");

            var retreivedUser = client.GetById("g46h5f15n11239862332v43238f382104");

            if (retreivedUser.IsError) { Assert.Fail(retreivedUser.ErrorMsg); }
            Assert.AreEqual("012345", retreivedUser.Result.employee_number);
        }

        [TestMethod]
        public void GetByQuery()
        {
            TableAPIClient<sys_user> client = new TableAPIClient<sys_user>("sys_user", "yourInstance", "user", "pass");

            string query = "u_agency.sys_id=e2353kf15n11239870249577c9tas67d^active=true";

            var retrievedUsers = client.GetByQuery(query);

            if (retrievedUsers.IsError) { Assert.Fail(retrievedUsers.ErrorMsg); }
            if (retrievedUsers.Result.Count == 0) { Assert.Fail("Expected users to be returned but none were."); }
            var testUser = retrievedUsers.Result.Where(x => x.last_name == "McArthur" & x.first_name == "Ryan").FirstOrDefault();
            Assert.AreEqual("012345", testUser.employee_number);
        }
    }
}
