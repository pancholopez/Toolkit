using System;
using System.ComponentModel;
using SlowProcess;

namespace ProcessHelper
{
    public class Class1
    {
        private BackgroundWorker _backgroundWorker;
        public Class1()
        {
            InitializeBackgroundWorker();
        }

        private void InitializeBackgroundWorker()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += OnDoWork;
            _backgroundWorker.RunWorkerCompleted += OnRunWorkerCompleted;

            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.ProgressChanged += _OnProgressChanged;
        }

        private void _OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int iterations = (int) e.Result;
        }

        private void OnDoWork(object sender, DoWorkEventArgs e)
        {
            int result = 0;
            int iterations = (int)e.Argument;

            LongRunningProcess slowProcess = new LongRunningProcess(iterations);
            foreach (var current in slowProcess)
            {
                result = current;
            }

            e.Result = result;
        }
    }
}
