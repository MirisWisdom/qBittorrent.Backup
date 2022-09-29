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

using System.IO;
using static System.Console;
using static System.Environment;

namespace qBittorrent.Backup
{
  internal static class Program
  {
    public static void Main(string[] args)
    {
      var archive = Archive.Generate();

      Message($"Backup: {archive.File.FullName}");

      foreach (var source in Source.All())
        try
        {
          var entries = archive.Compress(source);

          Message($"Backed up the following '{source.Type}' files:", '-');

          foreach (var entry in entries)
            WriteLine(entry.Name);
        }
        catch (DirectoryNotFoundException e)
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