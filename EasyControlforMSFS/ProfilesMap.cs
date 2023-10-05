using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;

namespace EasyControlforMSFS
{
    public class ProfilesMap
    {

        public List<ProfilesMapData> profiles_map;

        public class ProfilesMapData
        {
            public static int max_num_titles_mapped = 60;

            public string profile_name { get; set; }
            public string[] titles = new string[max_num_titles_mapped];
            public int nr_titles = 0;

            public void AddTitle(int title_nr, string title)
            {
                titles[title_nr] = title;
            }
        }

        public ProfilesMap()
        {
            profiles_map = new List<ProfilesMapData>();
        }

        /// <summary>
        /// This functions loads the current ProfilesMap.xml file with all definitions
        /// </summary>
        public ProfilesMap LoadProfilesMapFile(ProfilesMap profilesMap)
        {

            string profilesmap_file = AppDomain.CurrentDomain.BaseDirectory + "profilesmap.xml";
            int profile_id = 0;
            profilesMap.profiles_map.Clear();

            foreach (XElement level1Element in XElement.Load(@profilesmap_file).Elements())
            {
                //Debug.WriteLine(level1Element.Attribute("name").Value);
                profilesMap.profiles_map.Add(new ProfilesMap.ProfilesMapData() { profile_name= level1Element.Attribute("name").Value });
                int title_id = 0;
                Debug.WriteLine($"Profile: {level1Element.Attribute("name").Value} is profile nr {profile_id}");

                foreach (XElement level2Element in level1Element.Elements())
                {
                    string temp = level2Element.Value.Replace("&", "&amp");
                    
                    profilesMap.profiles_map[profile_id].AddTitle(title_id, temp);
                    profilesMap.profiles_map[profile_id].nr_titles += 1;
                    title_id += 1;
                    Debug.WriteLine($"Title added: {level2Element.Value} to profile {level1Element.Attribute("name").Value}");
                }
                profile_id += 1;
            }
            return profilesMap;
        }

        /// <summary>
        /// Creating the actual xml file
        /// </summary>
        public void SaveXML(ProfilesMap profilesMap)
        {
            string output_file = "";
            // we loop over the controls
            output_file += "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n";
            output_file += "<root>\r\n";
            for (int i = 0; i < profilesMap.profiles_map.Count; i++)
            {
                output_file += "\t<profile name=\"" + profilesMap.profiles_map[i].profile_name + "\">\r\n";
                Debug.WriteLine($"Aantal titels {profilesMap.profiles_map[i].nr_titles}");
                for (int j = 0; j < profilesMap.profiles_map[i].nr_titles; j++)
                {
                    output_file += "\t\t<title>" + profilesMap.profiles_map[i].titles[j] + "</title>\r\n";
                }
                output_file += "\t</profile>\r\n";
            }
            output_file += "</root>\r\n";
            string profilesmap_file = AppDomain.CurrentDomain.BaseDirectory + "profilesmap.xml";
            File.WriteAllTextAsync(profilesmap_file, output_file);
        }

        
    }

}
