using DotnetApiTemplate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DotnetApiTemplate.Core.Abstractions
{
  public interface ISendEventQueue
  {
    Task SendQueueAsync(SendQueueRequest paramQueue);
  }
}
