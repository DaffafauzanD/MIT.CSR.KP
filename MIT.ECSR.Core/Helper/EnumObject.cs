using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Core.Helper
{
    public enum ProgramStatusEnum
    {
        DRAFT = 1,
        OPEN = 2,
        ON_PROGRESS = 3,
        CLOSED = 4,
        FULL_BOOKED = 5,
        WAITING_VERIFIKASI = 6,
        WAITING_APPROVAL = 7,
        REJECT_VERIFIKASI = 8,
        REJECT_APPROVAL = 9,
    }
    public enum UsulanStatusEnum
    {
        DRAFT = 1,
        WAITING = 2,
        APPROVE = 3,
        REJECT = 4
    }
    public enum PenawaranStatusEnum
    {
        DRAFT=1,
        SUBMIT=2,
        CLOSED=3,
    }
    public enum ProgressStatusEnum
    {
        DRAFT = 0,
        WAITING = 1,
        APPROVE = 2,
        REJECT = 3
    }
}
