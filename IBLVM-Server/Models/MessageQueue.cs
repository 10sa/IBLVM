using IBLVM_Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IBLVM_Server.Models
{
	class MessageQueue
	{
		private readonly Queue<ClientMessage> queue = new Queue<ClientMessage>();
		private object blockingObject = new object();

		public void Enqueue(PacketType type, object payload)
		{
			lock (blockingObject)
			{
				queue.Enqueue(new ClientMessage(type, payload));
				Monitor.PulseAll(blockingObject);
			}
		}

		public ClientMessage Dequeue()
		{
			return queue.Dequeue();
		}

		public ClientMessage Peek()
		{
			return queue.Peek();
		}

		public void Wait(PacketType type)
		{ 
			lock(blockingObject)
			{
				do
					Monitor.Wait(blockingObject);
				while (type != queue.Peek().Type);
			}
		}
	}
}
