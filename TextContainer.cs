using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace GameAssetsManager
{
    class TextContainer
    {
        public Dictionary<string, Dictionary<string, string>> messages = new Dictionary<string, Dictionary<string, string>>();
        public BindingList<string> langs = new BindingList<string>();
        public BindingList<string> entries = new BindingList<string>();

        public void Load(string fname)
        {
            entries.Clear();
            langs.Clear();
            RZDBReader br = new RZDBReader(File.OpenRead(fname));
            int langCount = br.ReadSize();
            for (int i = 0; i < langCount; ++i)
                AddLanguage(br.ReadString());
            int messageCount = br.ReadSize();
            for (int i = 0; i < messageCount; ++i)
                AddMessageEntry(br.ReadString());
            for (int i = 0; i < langCount; ++i)
                for (int j = 0; j < messageCount; ++j)
                    messages[langs[i]][entries[j]] = br.ReadString();
            br.Close();
        }

        public void Save(string fname)
        {
            RZDBWriter bw = new RZDBWriter(File.Open(fname, FileMode.Create));
            bw.WriteSize(langs.Count);
            for (int i = 0; i < langs.Count; ++i)
                bw.Write(langs[i]);
            bw.WriteSize(entries.Count);
            for (int i = 0; i < entries.Count; ++i)
                bw.Write(entries[i]);
            for (int i = 0; i < langs.Count; ++i)
                for (int j = 0; j < entries.Count; ++j)
                    bw.Write(messages[langs[i]][entries[j]]);
            bw.Close();
        }

        public void AddLanguage(string lang, int idx = -1)
        {
            if (!messages.ContainsKey(lang))
            {
                messages[lang] = new Dictionary<string, string>();
                langs.Insert(idx < 0 ? langs.Count : idx, lang);
                foreach (string e in entries) messages[lang][e] = "";
            }
        }

        public void AddMessageEntry(string msg, int idx = -1)
        {
            entries.Insert(idx < 0 ? entries.Count : idx, msg);
            foreach (string l in langs) messages[l][msg] = "";
        }

        public void SetLanguage(int idx, string newlang)
        {
            messages[newlang] = messages[langs[idx]];
            langs[idx] = newlang;
        }

        public void SetEntry(int idx, string newmsg)
        {
            foreach (string l in langs)
                messages[l][newmsg] = messages[l][entries[idx]];
            entries[idx] = newmsg;
        }
    }
}
