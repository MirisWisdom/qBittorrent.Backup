<p>
    <h1 align='center'>
        qBittorrent.Backup
    </h1>
    <p align='center'>
        Back up your qBittorrent preferences and data.
    </p>
</p>

This program backs up all of your qBittorrent data to a ZIP file on your desktop, including but not limited to:

- preferences
- `.torrent` files
- logs
- statistics
- rss feeds
- etc.

The program is built with cross-platform support in mind, so it should work across Windows, Linux and macOS.

## Usage

### Back Up

Simply running the program will automatically back up your qBittorrent data to a ZIP file on your desktop.

```
qBittorrent.Backup.2022-09-29.16.38.00.zip
```

If you want to change the archive file, manually invoke the program like so:

```bat
.\qbt-backup.exe backup --archive "C:\Path\To\Backup.zip"
```

### Restore

From a CMD/PowerShell/Terminal window, manually invoke the program like so:

```bat
.\qbt-backup.exe restore --archive "C:\Users\Name\Desktop\qbt-backup.2022-09-30.18.50.45.zip"
```

Restoration will skip any existing files; if you want to overwrite them, use:

```bat
.\qbt-backup.exe restore --archive "C:\Users\Name\Desktop\qbt-backup.2022-09-30.18.50.45.zip" --overwrite
```

To spare you from any potential regrets, a backup will first be made if you try to overwrite.

If you haven't renamed or moved a previously created backup file from your desktop folder, you can skip providing the `--archive` parameter and let the program magically use the latest archive:

```bat
.\qbt-backup.exe restore
```