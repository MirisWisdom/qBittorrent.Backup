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

using System;
using System.IO;
using Mono.Options;
using static System.Console;
using static System.Environment;

namespace qBittorrent.Backup
{
  public static class Program
  {
    public static readonly CommandSet CommandSet = new("qbt-backup")
    {
      new Command("backup", "archive the current data to a new archive")
      {
        Options = new OptionSet
        {
          {
            "a|archive=", "optional full path where the archive should be created, e.g. ~/qbt-backup.zip",
            s => { Archive = new FileInfo(s).FullName; }
          }
        },
        Run = _ => { ArchiveData(); }
      },
      new Command("restore", "archive data from an existing archive")
      {
        Options = new OptionSet
        {
          {
            "a|archive=", "full path to the archive with backed up data, e.g. ~/qbt-backup.zip",
            s => { Restore = new FileInfo(s).FullName; }
          },
          {
            "overwrite", "overwrite existing files (this will create a backup first!)",
            s => Overwrite = !string.IsNullOrWhiteSpace(s)
          }
        },
        Run = _ => { RestoreData(); }
      }
    };

    public static string Archive   { get; set; } = string.Empty; /* custom path to the archive .zip  */
    public static string Restore   { get; set; } = string.Empty; /* custom path to the restore .zip  */
    public static bool   Overwrite { get; set; }                 /* overwrite during restore process */

    public static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        ArchiveData();
        Message("Successfully archived the data! Press any key to continue...", '~');
        ReadLine();
        Exit(0);
      }

      CommandSet.Run(args);
    }

    /**
     * Front-end for the data archive procedure.
     */
    private static void ArchiveData()
    {
      var archive = string.IsNullOrWhiteSpace(Archive)
        ? Backup.Archive.Generate()
        : new Archive(new FileInfo(Archive));

      Message($"Backup file: {archive.File.FullName}");

      foreach (var source in Source.All())
        try
        {
          var entries = archive.Compress(source);

          if (entries.Count <= 0)
            continue;

          Message($"Archived the following '{source.Type}' files:", '-');

          foreach (var entry in entries)
            WriteLine(entry.Name);
        }
        catch (Exception e)
        {
          Message(e.Message);
        }
    }

    /**
     * Front-end for the data restore procedure.
     */
    private static void RestoreData()
    {
      if (Overwrite)
        ArchiveData(); /* prevent regrettable decisions */

      var archive = string.IsNullOrWhiteSpace(Restore)
        ? Backup.Archive.Retrieve()
        : new Archive(new FileInfo(Restore));

      Message($"Restore file: {archive.File.FullName}");

      foreach (var source in Source.All())
        try
        {
          var restored = archive.Restore(source, Overwrite);

          if (restored.Count <= 0)
            continue;

          Message($"Restored the following '{source.Type}' files:", '-');

          foreach (var entry in restored)
            WriteLine(entry.Name);
        }
        catch (Exception e)
        {
          Message(e.Message);
        }
    }

    private static void Message(string message, char underline = '*')
    {
      Error.WriteLine($"{NewLine}{message}");
      Error.WriteLine(new string(underline, 72));
    }
  }
}