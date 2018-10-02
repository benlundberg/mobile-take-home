﻿using GuestlogixTestXF.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace GuestlogixTestXF
{
    public class ListViewModel : BaseViewModel
    {
        public ListViewModel(IEnumerable<ListItemViewModel> listItems, string title)
        {
            this.Title = title;
            this.listItems = new List<ListItemViewModel>(listItems);
        }

        private void ItemSelected(ListItemViewModel selectedListItem)
        {
			// When we have selected the item we can send a message that it's selected and pop modal
            Device.BeginInvokeOnMainThread(async () =>
            {
                // Send message that item is selected
                MessagingCenter.Instance.Send(this, AppConfig.MESSAGE_KEY_ITEM_SELECTED, SelectedListItem);

                await Navigation.PopModalAsync();
            });
        }

        private ICommand textChangedCommand;
        public ICommand TextChangedCommand => textChangedCommand ?? (textChangedCommand = new Command((param) =>
        {
			// Simple filtering to display what you search for
            if (string.IsNullOrEmpty((string)param))
            {
                FilterList?.Clear();
                return;
            }

            var filter = listItems.Where(x => x.Tags.ToLower().Trim().Contains(param.ToString().ToLower()));

            if (filter?.Any() == true)
            {
				// We only take 10 for better UI and performance
                FilterList = new ObservableCollection<ListItemViewModel>(filter.Take(10));
            }
        }));

        private ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new Command(async () =>
        {
            await Navigation.PopModalAsync();
        }));

        private ListItemViewModel selectedListItem;
        public ListItemViewModel SelectedListItem
        {
            get { return selectedListItem; }
            set
            {
                selectedListItem = value;
                ItemSelected(selectedListItem);
            }
        }

        public ObservableCollection<ListItemViewModel> FilterList { get; private set; }
        public string Title { get; set; }

        private readonly List<ListItemViewModel> listItems;
    }

    public class ListItemViewModel : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Tags { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
