using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System;

namespace TagFrenzy
{


    public class MultiTagManager
    {


        private static int nextId
        {
            get
            {
                if (editorTags.Count > 0)
                {
                    return editorTags.Max(e => Convert.ToInt32(e.Id)) + 1;
                }
                else
                {
                    return 1;
                }
            }
        }

        private static string tagLoadXmlFileName
        {
            get
            {
				return @"Tags";
            }
        }

        //Update save paths to work on windows and mac.  Thanks to Cristian for noticing the issue and providing a fix.
        private static char separator = Path.DirectorySeparatorChar;

        private static string tagXmlFileName
        {
            get
            {
                return Path.Combine(Application.dataPath, string.Format(@"TagFrenzy{0}Resources{0}Tags.xml", separator));
            }
        }

        private static string tagClassName
        {
            get
            {
                return Path.Combine(Application.dataPath, string.Format(@"TagFrenzy{0}Scripts{0}Core{0}Tags.cs", separator));
            }
        }

        private static List<EditorTag> editorTags;


        /// <summary>
        /// Is the tag name found in the existing list of tags
        /// </summary>
        /// <param name="editorTag"></param>
        /// <returns></returns>
        public static bool TagNameMatch(EditorTag editorTag)
        {
            return TagNameMatch(editorTag.Tag);
        }

        /// <summary>
        /// Is the tag name found in the existing list of tags
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool TagNameMatch(string name)
        {
            for (int i = 0; i < EditorTags.Count(); i++)
            {
                if (EditorTags[i].Tag.ToLower().Trim() == name.ToLower().Trim())
                {
                    return true;
                }
            }
            return false;
        }

        public static bool TagIdMatch(EditorTag editorTag)
        {
            return TagIdMatch(editorTag.Id);
        }

        public static bool TagIdMatch(string id)
        {
            for (int i = 0; i < EditorTags.Count(); i++)
            {
                if (EditorTags[i].Id.Trim() == id.Trim())
                {
                    return true;
                }
            }
            return false;
        }



        public static List<EditorTag> EditorTags
        {
            get
            {
                InitializeEditorTags();
                return editorTags;
            }
        }


        private static void InitializeEditorTags()
        {

            if (editorTags == null)
            {
                editorTags = new List<EditorTag>();
                var doc = Resources.Load(tagLoadXmlFileName) as TextAsset;
                if (doc == null)
                {
                    CreateEmptyXmlDoc();
                }
                else
                {
                    LoadXmlDoc();
                }
            }
        }



        private static void CreateEmptyXmlDoc()
        {
            XDocument xdoc = new XDocument(
                           new XElement("tags")
                       );
            try
            {
                xdoc.Save(tagXmlFileName);
            }
            catch (System.Exception ex)
            {

                Debug.LogError("Error saving tags xml file: " + ex.Message);
            }


        }

        private static void LoadXmlDoc()
        {
            var doc = Resources.Load(tagLoadXmlFileName) as TextAsset;
            XDocument xdoc = XDocument.Parse(doc.text);
            var xmltags = xdoc.Element("tags").Elements();
            foreach (XElement tag in xmltags)
            {
                EditorTag et = new EditorTag();
                et.Id = tag.Attribute("id").Value;
                et.Tag = tag.Value;
                editorTags.Add(et);
            }
        }



        public static string CleanName(string input)
        {
            input = Regex.Replace(input, "[^a-zA-Z0-9 -]", "");
            input = Regex.Replace(input, @"\s+", "");
            if (!IsValidIdentifier(input))
            {
                input = "";
            }
            return input;
        }

        private static void CreateTagClass(List<EditorTag> newTags)
        {
            List<string> contents = new List<string>();
            contents.Add("namespace TagFrenzy");
            contents.Add("{");
            contents.Add("public enum Tags");
            contents.Add("{");
            for (int i = 0; i < newTags.Count; i++)
            {
                //Strip out all non-alphanumeric items and spaces
                string cleanedTag = CleanName(newTags[i].Tag);

                if (!string.IsNullOrEmpty(cleanedTag))
                {
                    if (i != newTags.Count - 1)
                    {
                        contents.Add(cleanedTag + ",");
                    }
                    else
                    {
                        contents.Add(cleanedTag);
                    }
                }


            }
            contents.Add("}");
            contents.Add("}");

            try
            {
                File.WriteAllLines(tagClassName, contents.ToArray());
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error writing Tags enum" + ex.Message.ToString());
            }

        }

        public static void Update(List<EditorTag> newTags)
        {
            newTags = newTags.OrderBy(nt => nt.Tag).ToList();

            int id = nextId;

            XDocument xdoc = new XDocument(
                new XElement("tags")
                );
            foreach (EditorTag edTag in newTags)
            {
                if (!(edTag == null) && !string.IsNullOrEmpty(edTag.Tag))
                {
                    {

                        if (edTag.Id != "0")
                        {
                            xdoc.Element("tags").Add(
                                new XElement("tag",
                                    new XAttribute("id", edTag.Id),
                                    edTag.Tag)
                                );
                        }
                        else
                        {
                            xdoc.Element("tags").Add(
                            new XElement("tag",
                                new XAttribute("id", id.ToString()),
                                edTag.Tag)
                            );
                            id += 1;
                        }
                    }

                }
            }

            try
            {
                xdoc.Save(tagXmlFileName);
            }
            catch (System.Exception ex)
            {

                Debug.LogError("Error saving tags xml file: " + ex.Message);
            }

            CreateTagClass(newTags);
            
            //Force a reload the next time this is accessed
            editorTags = null;

        }

        /// <summary>
        /// From https://gist.github.com/LordDawnhunter/5245476
        /// </summary>
        private static bool IsValidIdentifier(string identifier)
        {
            if (string.IsNullOrEmpty(identifier)) return false;

            // C# keywords: http://msdn.microsoft.com/en-us/library/x53a06bb(v=vs.71).aspx
            var keywords = new[]
                       {
                           "abstract",  "event",      "new",        "struct",
                           "as",        "explicit",   "null",       "switch",
                           "base",      "extern",     "object",     "this",
                           "bool",      "false",      "operator",   "throw",
                           "breal",     "finally",    "out",        "true",
                           "byte",      "fixed",      "override",   "try",
                           "case",      "float",      "params",     "typeof",
                           "catch",     "for",        "private",    "uint",
                           "char",      "foreach",    "protected",  "ulong",
                           "checked",   "goto",       "public",     "unchekeced",
                           "class",     "if",         "readonly",   "unsafe",
                           "const",     "implicit",   "ref",        "ushort",
                           "continue",  "in",         "return",     "using",
                           "decimal",   "int",        "sbyte",      "virtual",
                           "default",   "interface",  "sealed",     "volatile",
                           "delegate",  "internal",   "short",      "void",
                           "do",        "is",         "sizeof",     "while",
                           "double",    "lock",       "stackalloc",
                           "else",      "long",       "static",
                           "enum",      "namespace",  "string"
                       };

            // definition of a valid C# identifier: http://msdn.microsoft.com/en-us/library/aa664670(v=vs.71).aspx
            const string formattingCharacter = @"\p{Cf}";
            const string connectingCharacter = @"\p{Pc}";
            const string decimalDigitCharacter = @"\p{Nd}";
            const string combiningCharacter = @"\p{Mn}|\p{Mc}";
            const string letterCharacter = @"\p{Lu}|\p{Ll}|\p{Lt}|\p{Lm}|\p{Lo}|\p{Nl}";
            const string identifierPartCharacter = letterCharacter + "|" +
                                                   decimalDigitCharacter + "|" +
                                                   connectingCharacter + "|" +
                                                   combiningCharacter + "|" +
                                                   formattingCharacter;
            const string identifierPartCharacters = "(" + identifierPartCharacter + ")+";
            const string identifierStartCharacter = "(" + letterCharacter + "|_)";
            const string identifierOrKeyword = identifierStartCharacter + "(" +
                                               identifierPartCharacters + ")*";
            var validIdentifierRegex = new Regex("^" + identifierOrKeyword + "$");
            var normalizedIdentifier = identifier.Normalize();

            // 1. check that the identifier match the validIdentifer regex and it's not a C# keyword
            if (validIdentifierRegex.IsMatch(normalizedIdentifier) && !keywords.Contains(normalizedIdentifier))
            {
                return true;
            }

            // 2. check if the identifier starts with @
            if (normalizedIdentifier.StartsWith("@") && validIdentifierRegex.IsMatch(normalizedIdentifier.Substring(1)))
            {
                return true;
            }

            // 3. it's not a valid identifier
            return false;
        }



    }
}


