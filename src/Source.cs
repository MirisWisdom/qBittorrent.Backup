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
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.Path;

namespace qBittorrent.Backup
{
  public class Source
  {
    public enum SourceType
    {
      Data,
      Preferences
    }

    public const string Base = "qBittorrent";

    public Source(SourceType type, DirectoryInfo directory)
    {
      Type      = type;
      Directory = directory;
    }

    public SourceType    Type      { get; }
    public DirectoryInfo Directory { get; }

    public static List<Source> All()
    {
      return new List<Source>
      {
        new(SourceType.Data, new DirectoryInfo(Combine(GetFolderPath(LocalApplicationData),   Base))),
        new(SourceType.Preferences, new DirectoryInfo(Combine(GetFolderPath(ApplicationData), Base)))
      };
    }
  }
}