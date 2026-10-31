using System;
using System.Windows.Forms;

namespace Tooling_WH_Sytems.Infrastructure
{
    /// <summary>Khóa thao tác và giữ form tồn tại trong lúc chờ tác vụ bất đồng bộ.</summary>
    internal sealed class BusyFormScope : IDisposable
    {
        private readonly Form _form;
        private readonly bool _wasEnabled;
        private readonly bool _wasUsingWaitCursor;
        private bool _disposed;

        public BusyFormScope(Form form)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            _form = form;
            _wasEnabled = form.Enabled;
            _wasUsingWaitCursor = form.UseWaitCursor;
            form.FormClosing += PreventClosing;
            form.Enabled = false;
            form.UseWaitCursor = true;
        }

        private void PreventClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _form.FormClosing -= PreventClosing;
            if (_form.IsDisposed) return;
            _form.UseWaitCursor = _wasUsingWaitCursor;
            _form.Enabled = _wasEnabled;
        }
    }
}
