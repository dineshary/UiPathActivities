﻿using System;
using System.Activities;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UiPathTeam.FTP.Activities.Properties;
using UiPathTeam.Shared.Activities;

namespace UiPathTeam.FTP.Activities
{
    public class DirectoryExists : ContinuableAsyncCodeActivity
    {
        [RequiredArgument]
        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.RemotePath))]
        public InArgument<string> RemotePath { get; set; }

        [LocalizedCategory(nameof(Resources.Output))]
        [LocalizedDisplayName(nameof(Resources.Exists))]
        public OutArgument<bool> Exists { get; set; }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            PropertyDescriptor ftpSessionProperty = context.DataContext.GetProperties()[WithFtpSession.FtpSessionPropertyName];
            IFtpSession ftpSession = ftpSessionProperty?.GetValue(context.DataContext) as IFtpSession;

            if (ftpSession == null)
            {
                throw new InvalidOperationException(Resources.FTPSessionNotFoundException);
            }

            bool exists = await ftpSession.DirectoryExistsAsync(RemotePath.Get(context), cancellationToken);

            return (asyncCodeActivityContext) =>
            {
                Exists.Set(asyncCodeActivityContext, exists);
            };
        }
    }
}
