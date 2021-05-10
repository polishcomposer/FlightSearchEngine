using FlightSE.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightSE.Models
{
    public class SearchQuery
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Way { get; set; }
        [Required]
        [Range(1,9)]
        public int Adults { get; set; }
        [Range(0, 8)]
        public int Children { get; set; }
        [Range(0, 4)]
        public int Infant { get; set; }
        public string Class { get; set; }
        public int Stopovers { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        [Required(ErrorMessage = "Depart field is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateFrom { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateTo { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime QueryDate { get; set; }


       [NotMapped]
        public JObject ImgAddress { get; set; }
        public void GetPicture(string GoTo)
        {
            string url = $"https://api.pexels.com/v1/search?query={GoTo}&per_page=1";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Headers["Authorization"] = "563492ad6f917000010000011ad16fa4180242eca06151e9eb89adbf";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var stringResponse = streamReader.ReadToEnd();
                ImgAddress = JObject.Parse(stringResponse);
            }
        }

      

}
   
}
