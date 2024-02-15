﻿using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace SCVRPatcher {

    internal class HostsFile {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        //private static readonly Regex EntryRegex = new Regex(@"^([\w.]+)\s+(.*)$");
        private static readonly Regex WhiteSpaceRegex = new Regex(@"\s+");
        internal static readonly IPAddress Null = new IPAddress(new byte[] { 0, 0, 0, 0 });
        internal static readonly IPAddress Localhost = new IPAddress(new byte[] { 127, 0, 0, 1 });
        internal static FileInfo HostFile = new FileInfo(Environment.GetEnvironmentVariable("windir") + @"\System32\drivers\etc\hosts");

        public List<HostsEntry> Entries = new();

        public HostsFile() {
            Load(HostFile);
        }

        public IEnumerable<HostsEntry> GetEntriesByDomain(string domain) => Entries.Where(e => e.Hostnames.Contains(domain));

        public IEnumerable<HostsEntry> GetEntryByIp(IPAddress ip) => Entries.Where(e => e.Ip.Equals(ip));

        public void DisableDomain(string domain, bool remove = false) {
            var entries = GetEntriesByDomain(domain);
            var removestring = remove ? "removing" : "disabling";
            if (entries.Count() < 1) {
                Logger.Debug($"{domain} not found in hosts file, skipping...");
                return;
            }
            if (entries.Count() > 1) {
                Logger.Warn($"{domain} found multiple times in hosts file, {removestring} all!");
                if (remove) entries.ToList().ForEach(e => Entries.Remove(e));
                else entries.ToList().ForEach(e => e.Enabled = false);
            } else {
                Logger.Info($"{domain} found in hosts file, {removestring} it now...");
                var entry = entries.First();
                entry.Enabled = false;
                if (remove) Entries.Remove(entry);
            }
        }

        public bool AddOrEnableDomain(string domain, IPAddress ip, string comment = null) {
            var entries = GetEntriesByDomain(domain);
            if (entries.Count() > 1) {
                Logger.Warn($"{domain} found multiple times in hosts file, disabling all except first!");
                entries.Skip(1).ToList().ForEach(e => e.Enabled = false);
                var entry = entries.First();
                entry.Enabled = true;
                entry.Ip = ip;
                entry.Comment = comment;
            }
            if (entries.Count() < 1) {
                Logger.Info($"{domain} not found in hosts file, adding it now...");
                Entries.Add(new HostsEntry() { Ip = ip, Hostnames = new List<string>() { domain }, Comment = comment });
            } else if (entries.First().Enabled) {
                Logger.Debug($"{domain} already enabled in hosts file, skipping...");
                return true;
            } else {
                Logger.Info($"{domain} found in hosts file, enabling it now...");
                var entry = entries.First();
                entry.Enabled = true;
                entry.Ip = ip;
                entry.Comment = comment;
            }
            return true;
        }

        public override string ToString() {
            return string.Join(Environment.NewLine, ToLines());
        }

        public List<string> ToLines() {
            var lines = new List<string>();
            foreach (var entry in Entries) {
                lines.Add(entry.ToString());
            }
            return lines;
        }

        public void Save(FileInfo file = null, bool backup = true) {
            file ??= HostFile;
            var original_file = new FileInfo(file.FullName); // This is a workaround for a bug in the File.MoveTo method idk valve pls fix
            Logger.Info($"Saving hosts file to {file.Quote()}");
            if (file.Exists && backup) {
                var backupFile = new FileInfo(file.FullName + ".bak");
                Logger.Info($"Hosts file already exists, creating backup at {backupFile.Quote()}");
                if (backupFile.Exists) backupFile.Delete();
                file.MoveTo(backupFile.FullName);
            } // Why is this buggy?
            File.WriteAllText(original_file.FullName, this.ToString());
            Logger.Info($"Saved hosts file to {original_file.Quote()}");
        }

        public List<HostsEntry> Load(FileInfo file = null) {
            file ??= HostFile;
            if (!file.Exists) {
                Logger.Error($"Hosts file not found: {file.Quote()}");
                return null;
            }
            Logger.Trace($"Loading hosts file from {file.Quote()}");
            var lines = File.ReadAllLines(file.FullName);
            Logger.Info($"Loaded hosts file with {lines.Length} lines.");
            Entries.Clear();
            foreach (var _line in lines) {
                var line = _line.Trim();
                var entry = new HostsEntry();
                if (string.IsNullOrWhiteSpace(line)) {
                    Logger.Trace($"Line is empty or whitespace: {line}");
                    entry.Empty = true;
                    Entries.Add(entry);
                    continue;
                }
                if (line.StartsWith("#")) {
                    Logger.Trace($"Line starts with #, treating as disabled entry: {line}");
                    entry.Enabled = false;
                    line = line.Substring(1).Trim();
                }
                var parts = WhiteSpaceRegex.Split(line);
                if (parts.Length < 2) {
                    Logger.Trace($"Line is too short: {line}");
                    entry.Comment = line.Replace("#", string.Empty).Trim();
                    Entries.Add(entry);
                    continue;
                }
                var success = IPAddress.TryParse(parts[0], out var ip);
                if (!success) {
                    Logger.Trace($"Not a valid IP: {parts[0]}");
                    entry.Comment = line;
                    Entries.Add(entry);
                    continue;
                }
                entry.Ip = ip;
                for (var i = 1; i < parts.Length; i++) {
                    if (parts[i].StartsWith("#")) {
                        entry.Comment = string.Join(' ', parts.Skip(i)).Replace("#", string.Empty).Trim();
                        break;
                    }
                    entry.Hostnames.Add(parts[i]);
                }
                Entries.Add(entry);
            }
            Logger.Debug($"Loaded {Entries.Count} entries from hosts file ({Entries.Where(e => !e.Enabled).Count()} disabled, {Entries.Where(e => e.Empty).Count()} empty)");
            return Entries;
        }
    }

    internal class HostsEntry {
        public bool Enabled { get; set; } = true;
        public bool Empty { get; set; } = false;
        public IPAddress? Ip { get; set; }
        public List<string> Hostnames { get; set; } = new();
        public string? Comment { get; set; }

        public override string ToString() {
            var line = "";
            if (!Enabled) line += "# ";
            if (Empty) { return string.Empty; }
            if (Ip is null && !string.IsNullOrWhiteSpace(Comment)) return $"# {Comment}";
            if (Ip is not null) line += Ip.ToString();
            if (Hostnames.Count > 0) line += "\t" + string.Join(' ', Hostnames);
            if (Comment is not null) line += "\t# " + Comment;
            return line;
        }
    }
}