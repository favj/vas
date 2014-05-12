using Com.VizApp.VizApp.TestClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

namespace Com.VizApp.VizApp.TestClient
{
    public partial class VizAppClient : Form
    {
        HttpWebRequest request;
        CookieContainer cookies;
        public static string key = string.Empty;
        string payloadData = string.Empty;
        string hostName = string.Empty;
        private const string LOCALHOST = "http://localhost:49898/vizapp";

        public VizAppClient()
        {
            InitializeComponent();
            hostName = LOCALHOST;
        }

        private void cbServerURL_SelectedIndexChanged(object sender, EventArgs e)
        {
            hostName = ((ComboBox)sender).SelectedItem.ToString();
        }

        private void VizAppClient_Load(object sender, EventArgs e)
        {
            cookies = new CookieContainer();
            cmbURL.Items.Add(LOCALHOST);
            cmbURL.SelectedIndex = 0;
        }

        private void btSaveFBDatas_Click(object sender, EventArgs e)
        {
            string url = "/api/VizFB/SaveFBDetails";
            FBData fbData = new FBData()
            {
                User = new FBUser
                {
                    id = "12563162721",
                    FirstName = "Vijay",
                    Gender = "Male",
                    LastName = "Ramesh",
                    Link = "http://192.81.210.146",
                    Locale = "en-US",
                    Name = "Vijay",
                    ProfileUrl = "http://facebook.com/unityteam",
                    TimeZone = "GMT+5.30",
                    UpdatedTime = DateTime.Now,
                    UserName = "Unity Team",
                    Verified = true,
                    Work = new List<FBEmployer>
                    {
                        new FBEmployer()
                        {
                            id="1237824",
                            Name="OFS"
                        },
                        new FBEmployer()
                        {
                            id="1237825",
                            Name="FA"
                        }
                    }
                },
                Friends = new FBFriends
                {
                    Friends = new List<FBFriend>
                    {
                        new FBFriend()
                        {
                            id="1725371",
                            Email="sandy@gmail.com",
                            ImageURL="http://graph.facebook.com/t/img_724872.jpg",
                            Lat="12.523623",
                            Lng="20.254624",
                            Name="Santhosh",
                            PhoneNumber="1823912121",
                            ProfileURL="http://facebook.com/sandy"
                        },
                        new FBFriend()
                        {
                            id="1725372",
                            Email="sexy@gmail.com",
                            ImageURL="http://graph.facebook.com/t/img_724873.jpg",
                            Lat="12.553623",
                            Lng="20.256624",
                            Name="Sexy",
                            PhoneNumber="1823912154",
                            ProfileURL="http://facebook.com/sexy"
                        }
                    }
                }
            };
            //string json = @"{"Friends":{"Friends":[{"Email":"Rakesh Ks@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash3/t5/370187_537462998_1257659137_q.jpg","Lat":"12.120","Lng":"12.120","Name":"Rakesh Ks","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Rakesh Ks","id":"537462998","Selected":true},{"Email":"Donn Kabiraj@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash3/t5/623595_610332030_1326593215_q.jpg","Lat":"12.121","Lng":"12.121","Name":"Donn Kabiraj","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Donn Kabiraj","id":"610332030","Selected":true},{"Email":"Purna Kumar@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash2/t5/370138_1084553779_897804858_q.jpg","Lat":"12.122","Lng":"12.122","Name":"Purna Kumar","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Purna Kumar","id":"1084553779","Selected":true},{"Email":"R.Ratish Kumar@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-frc1/t5/371795_1320114662_425662618_q.jpg","Lat":"12.123","Lng":"12.123","Name":"R.Ratish Kumar","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/R.Ratish Kumar","id":"1320114662","Selected":true},{"Email":"Ranjith Selvaraj@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-frc1/t5/372425_1377566050_457968711_q.jpg","Lat":"12.124","Lng":"12.124","Name":"Ranjith Selvaraj","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Ranjith Selvaraj","id":"1377566050","Selected":true},{"Email":"Pramod Kumar Bugudai@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash2/t5/273655_1440228172_664609_q.jpg","Lat":"12.125","Lng":"12.125","Name":"Pramod Kumar Bugudai","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Pramod Kumar Bugudai","id":"1440228172","Selected":true},{"Email":"Vijay Anand Mca@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-frc1/t5/371080_100000056538048_459916951_q.jpg","Lat":"12.126","Lng":"12.126","Name":"Vijay Anand Mca","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Vijay Anand Mca","id":"100000056538048","Selected":true},{"Email":"Prem Kumar Aol@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash2/t5/1116376_100000553569379_1770598516_q.jpg","Lat":"12.127","Lng":"12.127","Name":"Prem Kumar Aol","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Prem Kumar Aol","id":"100000553569379","Selected":true},{"Email":"Suresh Elumalai@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-prn2/t5/1119178_100000662701952_1950039565_q.jpg","Lat":"12.128","Lng":"12.128","Name":"Suresh Elumalai","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Suresh Elumalai","id":"100000662701952","Selected":true},{"Email":"Stephen Arputharaj@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-prn2/t5/1118303_100000689750777_132852229_q.jpg","Lat":"12.129","Lng":"12.129","Name":"Stephen Arputharaj","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Stephen Arputharaj","id":"100000689750777","Selected":true},{"Email":"Praveen Kumar U@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash3/t5/23221_100000698491896_1308_q.jpg","Lat":"12.1210","Lng":"12.1210","Name":"Praveen Kumar U","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Praveen Kumar U","id":"100000698491896","Selected":true},{"Email":"Manikandan Murugesan@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-prn2/t5/186021_100000773228368_842357115_q.jpg","Lat":"12.1211","Lng":"12.1211","Name":"Manikandan Murugesan","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Manikandan Murugesan","id":"100000773228368","Selected":true},{"Email":"Venugopal Sathyanarayanan@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-prn1/t5/173607_100000852425588_1905925208_q.jpg","Lat":"12.1212","Lng":"12.1212","Name":"Venugopal Sathyanarayanan","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Venugopal Sathyanarayanan","id":"100000852425588","Selected":true},{"Email":"Santhosh Kumar Chandran@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash1/t5/275728_100000992067997_1107323127_q.jpg","Lat":"12.1213","Lng":"12.1213","Name":"Santhosh Kumar Chandran","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Santhosh Kumar Chandran","id":"100000992067997","Selected":true},{"Email":"Shaak Pathak@gmail.com","ImageURL":"https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash2/t5/1119555_100001390244945_793606983_q.jpg","Lat":"12.1214","Lng":"12.1214","Name":"Shaak Pathak","PhoneNumber":"3467738468","ProfileURL":"http://facebook.com/Shaak Pathak","id":"100001390244945","Selected":true}]},"User":{"FirstName":"Fixture","Gender":"male","LastName":"Apps","Link":"http://facebook.com/unityteam","Locale":"en_US","Name":"Fixture Apps","ProfileUrl":"https://www.facebook.com/fixture.unity","work":[{"employer":{"id":"19549406249","name":"Tata Consultancy Services"}}],"UpdatedTime":"28 Dec 2013 14:11:06","UserName":"fixture.unity","id":"100006286272328","Verified":true,"TimeZone":5.5}}";
            fbData = getFBData();
            bool result = InvokeService<FBData>(fbData, url, "POST", false, false);
        }

        private FBData getFBData()
        {
            FBData data = new FBData();
            data.User = new FBUser();
            data.User.id = "100006286272328";
            data.User.Work = new List<FBEmployer>();
            data.User.FirstName = "Fixture";
            data.User.UserName = "fixture.unity";
            data.User.TimeZone = "5.5";
            data.User.Verified = true;
            data.User.Locale = "en_US";
            data.User.Link = "https://www.facebook.com/fixture.unity";
            data.User.Name = "Fixture Apps";
            data.User.LastName = "Apps";
            data.User.Gender = "male";
            data.User.UpdatedTime = DateTime.Parse("28 Dec 2013 14:11:06");
            data.User.ProfileUrl = "http://facebook.com/unityteam";

            List<FBFriend> frnds = new List<FBFriend>();
            frnds.Add(CreateFriend("537462998", "Rakesh Ks", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash3/t5/370187_537462998_1257659137_q.jpg", "http://facebook.com/Rakesh Ks", "12.12", "12.12", "Rakesh Ks@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("610332030", "Donn Kabiraj", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash3/t5/623595_610332030_1326593215_q.jpg", "http://facebook.com/Donn Kabiraj", "12.12", "12.12", "Donn Kabiraj@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("1084553779", "Purna Kumar", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash2/t5/370138_1084553779_897804858_q.jpg", "http://facebook.com/Purna Kumar", "12.12", "12.12", "Purna Kumar@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("1320114662", "R.Ratish Kumar", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-frc1/t5/371795_1320114662_425662618_q.jpg", "http://facebook.com/R.Ratish Kumar", "12.12", "12.12", "R.Ratish Kumar@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("1377566050", "Ranjith Selvaraj", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-frc1/t5/372425_1377566050_457968711_q.jpg", "http://facebook.com/Ranjith Selvaraj", "12.12", "12.12", "Ranjith Selvaraj@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("1440228172", "Pramod Kumar Bugudai", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash2/t5/273655_1440228172_664609_q.jpg", "http://facebook.com/Pramod Kumar Bugudai", "12.12", "12.12", "Pramod Kumar Bugudai@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("100000056538048", "Vijay Anand Mca", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-frc1/t5/371080_100000056538048_459916951_q.jpg", "http://facebook.com/Vijay Anand Mca", "12.12", "12.12", "Vijay Anand Mca@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("100000553569379", "Prem Kumar Aol", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash2/t5/1116376_100000553569379_1770598516_q.jpg", "http://facebook.com/Prem Kumar Aol", "12.12", "12.12", "Prem Kumar Aol@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("100000662701952", "Suresh Elumalai", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-prn2/t5/1119178_100000662701952_1950039565_q.jpg", "http://facebook.com/Suresh Elumalai", "12.12", "12.12", "Suresh Elumalai@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("100000689750777", "Stephen Arputharaj", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-prn2/t5/1118303_100000689750777_132852229_q.jpg", "http://facebook.com/Stephen Arputharaj", "12.12", "12.12", "Stephen Arputharaj@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("100000698491896", "Praveen Kumar U", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash3/t5/23221_100000698491896_1308_q.jpg", "http://facebook.com/Praveen Kumar U", "12.12", "12.12", "Praveen Kumar U@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("100000773228368", "Manikandan Murugesan", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-prn2/t5/186021_100000773228368_842357115_q.jpg", "http://facebook.com/Manikandan Murugesan", "12.12", "12.12", "Manikandan Murugesan@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("100000852425588", "Venugopal Sathyanarayanan", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-prn1/t5/173607_100000852425588_1905925208_q.jpg", "http://facebook.com/Venugopal Sathyanarayanan", "12.12", "12.12", "Venugopal Sathyanarayanan@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("100000992067997", "Santhosh Kumar Chandran", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash1/t5/275728_100000992067997_1107323127_q.jpg", "http://facebook.com/Santhosh Kumar Chandran", "12.12", "12.12", "Santhosh Kumar Chandran@gmail.com", "3467738468", true));
            frnds.Add(CreateFriend("100001390244945", "Shaak Pathak", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash2/t5/1119555_100001390244945_793606983_q.jpg", "http://facebook.com/Shaak Pathak", "12.12", "12.12", "Shaak Pathak@gmail.com", "3467738468", true));

            data.Friends = new FBFriends();
            data.Friends.Friends = frnds;
            return data;
        }

        private FBFriend CreateFriend(string id, string name, string imgURL, string profileURL, string lat, string lng, string email, string phNo, bool selected)
        {
            FBFriend friend = new FBFriend();
            friend.id = id;
            friend.Name = name;
            friend.ImageURL = imgURL;
            friend.ProfileURL = profileURL;
            friend.Lat = lat;
            friend.Lng = lng;
            friend.Email = email;
            friend.PhoneNumber = phNo;
            friend.Selected = selected;
            return friend;
        }

        private bool InvokeService<T>(T input, string url, string httpMethod, bool isGetConfig, bool isHeaderRequired)
        {
            request = (HttpWebRequest)WebRequest.Create(hostName + url);
            request.CookieContainer = cookies;
            request.ContentType = "application/json";
            request.Method = httpMethod;
            request.Timeout = System.Threading.Timeout.Infinite;

            if (isHeaderRequired)
            {
                request.Headers.Add("NotificationId", "notification1");
                request.Headers.Add("DeviceOS", "Android JellyBean");
                request.Headers.Add("DeviceType", "Galaxy");
                request.Headers.Add("AppVersion", "1.5");
                request.Headers.Add("InstallationId", "1");
                //request.Headers.Add("True_Client_IP", "Mobile App");
            }
            try
            {
                if ("POST".Equals(httpMethod) || "DELETE".Equals(httpMethod))
                {
                    Stream stream = request.GetRequestStream();
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    serializer.WriteObject(stream, input);
                    stream.Close();
                }

                if (isGetConfig)
                {
                    HttpWebResponse webResponse = request.GetResponse() as HttpWebResponse;
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(string));
                    string dynamicText = (string)serializer.ReadObject(webResponse.GetResponseStream());

                    key = dynamicText;
                    payloadData = EncryptUtil.EncryptString("LIBERTY.BUSINESS2BUSINESS", key);
                }
                else
                {
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string output = (string)reader.ReadToEnd();
                        txtResponse.Text = "Response : " + output;
                        if (isGetConfig)
                        {
                            payloadData = EncryptUtil.EncryptString("LIBERTY.BUSINESS2BUSINESS", output);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    txtResponse.Text = "Response : " + reader.ReadToEnd();
                }
                return false;
            }
            return true;
        }

        private void btGetSettings_Click(object sender, EventArgs e)
        {
            string url = "/api/Viz/GetSettings";
            bool result = InvokeService<string>(string.Empty, url, "GET", false, false);
        }

        private void btUpdateSettings_Click(object sender, EventArgs e)
        {
            string url = "/api/Viz/UpdateSettings";
            Settings settings = new Settings()
            {
                LocationUpdate = 25,
                AutoLogin = true,
                Sound = false,
                Vibrate = true
            };
            bool result = InvokeService<Settings>(settings, url, "POST", false, false);
        }

        private void btUpdateLocation_Click(object sender, EventArgs e)
        {
            string url = "/api/Viz/UpdateUserLocation";
            Location location = new Location()
            {
                Latitude = "12.214525",
                Longitude = "80.3247928"
            };
            bool result = InvokeService<Location>(location, url, "POST", false, false);
        }

        private void btGetFriends_Click(object sender, EventArgs e)
        {
            string url = "/api/VizFriends/GetFriendsList";
            bool result = InvokeService<string>(string.Empty, url, "GET", false, false);
        }

        private void btUpdateFriends_Click(object sender, EventArgs e)
        {
            string url = "/api/VizFriends/UpdateSelectedFriends";
            FBFriends Friends = new FBFriends
            {
                Friends = new List<FBFriend>
                {
                    new FBFriend()
                    {
                        id="1725371",
                        Email="sandy@gmail.com",
                        ImageURL="http://graph.facebook.com/t/img_67853.jpg",
                        Lat="12.523623",
                        Lng="20.254624",
                        Name="Santhosh",
                        PhoneNumber="1823967563",
                        ProfileURL="http://facebook.com/sandy",
                        Selected = false
                    },
                    new FBFriend()
                    {
                        id="1725372",
                        Email="praveen@gmail.com",
                        ImageURL="http://graph.facebook.com/t/img_724873.jpg",
                        Lat="12.553623",
                        Lng="20.256624",
                        Name="Praveen",
                        PhoneNumber="1823912154",
                        ProfileURL="http://facebook.com/sexy",
                        Selected = true
                    }
                }
            };
            List<FBFriend> frnds = new List<FBFriend>();
            frnds.Add(CreateFriend("537462998", "Rakesh Ks", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash3/t5/370187_537462998_1257659137_q.jpg", "http://facebook.com/Rakesh Ks", "12.12", "12.12", "Rakesh Ks@gmail.com", "3467738468", false));
            frnds.Add(CreateFriend("610332030", "Donn Kabiraj", "https://m.ak.fbcdn.net/profile.ak/hprofile-ak-ash3/t5/623595_610332030_1326593215_q.jpg", "http://facebook.com/Donn Kabiraj", "12.12", "12.12", "Donn Kabiraj@gmail.com", "3467738468", false));
            Friends.Friends = frnds;
            bool result = InvokeService<FBFriends>(Friends, url, "POST", false, false);
        }

        private void btLogout_Click(object sender, EventArgs e)
        {
            string url = "/api/VizSecurity/Logout";
            bool result = InvokeService<string>(string.Empty, url, "GET", false, false);
        }

        private void btRegister_Click(object sender, EventArgs e)
        {
            string url = "/api/Viz/Register";
            User User = new User
            {
                FirstName = "Vijay",
                LastName = "R",
                Email = "vijay1022@yahoo.co.in",
                Password = "Vijay123"
            };
            bool result = InvokeService<User>(User, url, "POST", false, false);
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            string url = "/api/VizSecurity/Login";
            Credentials Creds = new Credentials
            {
                Email = "vijay1022@yahoo.co.in",
                Password = "Vijay123"
            };
            bool result = InvokeService<Credentials>(Creds, url, "POST", false, false);
        }
    }
}
