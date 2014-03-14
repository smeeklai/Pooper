using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poopor
{
    public static class ToolkitExtensions
    {
        public static Task<CustomMessageBoxResult> ShowAsync(this CustomMessageBox source)
        {

            var completion = new TaskCompletionSource<CustomMessageBoxResult>();
            // wire up the event that will be used as the result of this method
            EventHandler<DismissedEventArgs> dismissed = null;

            dismissed += (sender, args) =>
            {

                completion.SetResult(args.Result);

                // make sure we unsubscribe from this!

                source.Dismissed -= dismissed;

            };
            source.Dismissed += dismissed;
            source.Show();
            return completion.Task;
        }
    }
}
