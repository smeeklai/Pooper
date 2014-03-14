using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Poopor.Resources;

namespace Poopor
{
    public partial class AdditionalHealthInfomation2 : PhoneApplicationPage
    {
        public AdditionalHealthInfomation2()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            var userLastestResultAndRecommendation = SessionManagement.GetUserLastestResultsAndRecommendation();
            var userCancerSignMsg = userLastestResultAndRecommendation["UserCancerSignMsg"] as List<string>;
            if ((Boolean)stomachCramps_checkBox.IsChecked)
                userCancerSignMsg.Add(stomachCramps_checkBox.Content as string);
            if ((Boolean)flatulence_checkBox.IsChecked)
                userCancerSignMsg.Add(flatulence_checkBox.Content as string);
            if ((Boolean)andominalPain_checkBox.IsChecked)
                userCancerSignMsg.Add(andominalPain_checkBox.Content as string);
            if ((Boolean)vomiting_checkBox.IsChecked)
                userCancerSignMsg.Add(vomiting_checkBox.Content as string);
            if ((Boolean)debility_checkBox.IsChecked)
                userCancerSignMsg.Add(debility_checkBox.Content as string);
            if ((Boolean)pallor_checkBox.IsChecked)
                userCancerSignMsg.Add(pallor_checkBox.Content as string);
            if ((Boolean)anorexia_checkBox.IsChecked)
                userCancerSignMsg.Add(anorexia_checkBox.Content as string);
            if ((Boolean)weightLoss_checkBox.IsChecked)
                userCancerSignMsg.Add(weightLoss_textBlock.Text);
            if ((Boolean)fecesSize_checkBox.IsChecked)
                userCancerSignMsg.Add(fecesSize_textBlock.Text);
            if ((Boolean)gropingTumor_checkBox.IsChecked)
                userCancerSignMsg.Add(gropingTumor_textBlock.Text);
            if ((Boolean)feelingNotEmptying_checkBox.IsChecked)
                userCancerSignMsg.Add(feelingNotEmptying_textBlock.Text);
        }

    }
}