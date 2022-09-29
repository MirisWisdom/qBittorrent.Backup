/**
 * Copyright (C) 2022 Emilian Roman
 * 
 * This file is part of qBittorrent.Backup.
 * 
 * qBittorrent.Backup is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * qBittorrent.Backup is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with qBittorrent.Backup.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using static System.IO.Path;
using static System.IO.SearchOption;
using static System.IO.Compression.ZipArchiveMode;
using static System.DateTime;
using static System.Environment;
using static System.Environment.SpecialFolder;

namespace qBittorrent.Backup
{
  public class Archive
  {
    private static readonly string ZipPrefix    = "qbt-backup";
    private static readonly string ZipDirectory = GetFolderPath(Desktop);

    public Archive(FileInfo file)
    {
      File = file;
    }

    public FileInfo File { get; }

    public static Archive Generate()
    {
      var name = $"{ZipPrefix}.{Now:yyyy-MM-dd.HH.mm.ss}.zip";
      var path = new FileInfo(Combine(ZipDirectory, name));
      return new Archive(path);
    }

    public static Archive Retrieve()
    {
      return new Archive(new DirectoryInfo(ZipDirectory).GetFiles($"{ZipPrefix}.*")
        .Last());
    }

    public List<FileInfo> Compress(Source source)
    {
      if (!source.Directory.Exists)
        throw new DirectoryNotFoundException("Source directory does not exist!");

      var entries = new List<FileInfo>();

      using ZipArchive zip = ZipFile.Open(File.FullName, Update);
      foreach (var file in source.Directory.GetFiles("*", AllDirectories))
      {
        var path = GetRelativePath(source.Directory.FullName, file.FullName);
        zip.CreateEntryFromFile(file.FullName, Combine(source.Type.ToString().ToLower(), path));
        entries.Add(file);
      }

      return entries;
    }
  }
}
