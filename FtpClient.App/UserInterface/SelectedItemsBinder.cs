﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FtpClient.UserInterface
{
    public class SelectedItemsBinder
    {
        private ListView _listBox;
        private IList _collection;

        public SelectedItemsBinder(ListView listBox, IList collection)
        {
            _listBox = listBox;
            _collection = collection;

            _listBox.SelectedItems.Clear();

            foreach (var item in _collection)
            {
                _listBox.SelectedItems.Add(item);
            }
        }

        public void Bind()
        {
            _listBox.SelectionChanged += ListBox_SelectionChanged;

            if (_collection is INotifyCollectionChanged)
            {
                var observable = (INotifyCollectionChanged)_collection;
                observable.CollectionChanged += Collection_CollectionChanged;
            }
        }

        public void UnBind()
        {
            if (_listBox != null)
                _listBox.SelectionChanged -= ListBox_SelectionChanged;

            if (_collection != null && _collection is INotifyCollectionChanged)
            {
                var observable = (INotifyCollectionChanged)_collection;
                observable.CollectionChanged -= Collection_CollectionChanged;
            }
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems ?? new object[0])
            {
                if (!_listBox.SelectedItems.Contains(item))
                    _listBox.SelectedItems.Add(item);
            }
            foreach (var item in e.OldItems ?? new object[0])
            {
                _listBox.SelectedItems.Remove(item);
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems ?? new object[0])
            {
                if (!_collection.Contains(item))
                    _collection.Add(item);
            }

            foreach (var item in e.RemovedItems ?? new object[0])
            {
                _collection.Remove(item);
            }
        }
    }
}
