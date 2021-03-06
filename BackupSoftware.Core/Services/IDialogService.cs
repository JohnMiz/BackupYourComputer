﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSoftware.Core.Services
{
	 /// <summary>
	 /// Interface for managing dialog services
	 /// </summary>
	 public interface IDialogService
	 {
		  string SelectFolder(string title, string initialDir);
		  IEnumerable<string> SelectFolders(string title, string initialDir);

		  void ShowMessageBox(string message);

		  bool ShowYesNoMessageBox(string message, string title);

	 }
}
