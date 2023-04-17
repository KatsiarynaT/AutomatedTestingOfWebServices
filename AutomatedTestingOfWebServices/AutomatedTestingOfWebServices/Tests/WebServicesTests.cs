using AutomatedTestingOfWebServices.Json;
using Newtonsoft.Json;
using System.Net;

namespace AutomatedTestingOfWebServices.Tests
{
    [TestFixture]
    [Parallelizable]
    public class WebServicesTests
    {
        public HttpWebResponse response;

        public string MakeRequest()
        {
            string responseBody = String.Empty;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://jsonplaceholder.typicode.com/users");
            request.Method = "GET";

            response = (HttpWebResponse)request.GetResponse();
            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader r = new StreamReader(s))
                {
                    responseBody = r.ReadToEnd();
                }

                return responseBody;
            }
        }

        [Test]
        public void GetHttpStatusCode()
        {
            //Arrange
            int expectedStatusCode = 200;
            string expectedDescription = "OK";

            //Act
            var actualStatusCode = Convert.ToInt32(response.StatusCode);
            var actualDescription = response.StatusDescription;

            //Assert
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
            Assert.AreEqual(expectedDescription, actualDescription);
        }

        [Test]
        public void GetHttpResponseHeader()
        {
            //Arrange
            string expectedHeader = "Content-Type";
            string expectedHeaderValue = "application/json; charset=utf-8";

            //Act
            var actualHeaderValue = response.Headers.GetValues(expectedHeader).First();

            //Assert
            Assert.AreEqual(expectedHeaderValue, actualHeaderValue);
        }

        [Test]
        public void GetHttpResponseBody()
        {
            //Arrange
            int expectedNumberOfUsers = 10;
            var response = MakeRequest();

            //Act
            List<User> users = JsonConvert.DeserializeObject<List<User>>(response);
            int actualNumberOfUsers = users.Count;

            //Assert
            Assert.AreEqual(expectedNumberOfUsers, actualNumberOfUsers);
        }
    }
}