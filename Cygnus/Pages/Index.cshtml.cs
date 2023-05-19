
using System.Linq;
using System.Net;
using System.Text;
using Cygnus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages
{
    public class IndexModel : PageModel
    {
        //list ]of products
        public List<Product> Products = new List<Product>();

        private readonly IProductRepository _productRepository;

        public IndexModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void OnPostSearch(string searchText)
        {
            Products = _productRepository.GetAllProducts().ToList();
            var results = Products.Where(h => h.Name.Contains(searchText) || h.Description.Contains(searchText));
            Products = results.ToList();
            //print json from "https://api.novaposhta.ua/v2.0/json/"
            string apiUrl = "https://api.novaposhta.ua/v2.0/json/";
            string requestData = "{ \"modelName\": \"Address\", \"calledMethod\": \"getCities\", \"methodProperties\": { \"FindByString\": \"київ\" }, \"apiKey\": \"YOUR_API_KEY_HERE\" }";

            // Set up the request
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/json";

            // Write the request data to the request stream
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream())) {
                writer.Write(requestData);
            }

            // Get the response and read the JSON data
            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse()) {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string json = readStream.ReadToEnd();
                Console.WriteLine(json);
            }

        }

        public void OnGet()
        {
            Products = _productRepository.GetAllProducts().ToList();
        }
    }

    }
