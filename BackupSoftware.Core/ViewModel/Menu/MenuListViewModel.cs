﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupSoftware.Core
{
	 /// <summary>
	 /// The view model for the items of the menu
	 /// </summary>
	 public class MenuListViewModel : ViewModelBase
	 {
		  /// <summary>
		  /// A list of all the items in the menu
		  /// </summary>
		  public List<MenuItemViewModel> Items { get; set; }

		  /// <summary>
		  /// The selected item
		  /// </summary>

		  private MenuItemViewModel _SelectedItem;

		  public MenuItemViewModel SelectedItem
		  {
			   get { return _SelectedItem; }
			   set {
					if (_SelectedItem == value)
						 return;

					_SelectedItem = value;

					SelectItem();

					OnPropertyChanged(nameof(SelectedItem));
					
			   }
		  }

		  private int _SelectedIndex;

		  public int SelectedIndex
		  {
			   get { return _SelectedIndex; }
			   set { if (_SelectedIndex == value) return; _SelectedIndex = value; OnPropertyChanged(nameof(SelectedIndex)); }
		  }

		  private void SelectItem()
		  {
			   _SelectedItem.IsSelected = true;

			   foreach (var item in Items)
			   {
					if (item != SelectedItem)
						 item.IsSelected = false;
			   }

			   _SelectedItem.GoToView();
		  }

		  /// <summary>
		  /// Default Constructor
		  /// </summary>
		  public MenuListViewModel()
		  {

			   Items = new List<MenuItemViewModel>
			   {
					new MenuItemViewModel
					{
						 IconCode = "\uf574",
						 Title = "Backup",
						 ViewModel = ViewModelLocator.DetailsViewModel,
						 IsSelected = true
					},
					new MenuItemViewModel
					{
						 IconCode = "\uf108",
						 Title = "Screenshots",
						 ViewModel = ViewModelLocator.ScreenshotsViewModel
					},
					new MenuItemViewModel
					{
						 IconCode = "\uf013",
						 Title = "Settings",
						 ViewModel = null
					}
			   };

		  }


	 }
}
