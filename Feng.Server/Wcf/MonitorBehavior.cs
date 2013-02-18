using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;

namespace Feng.Server.Wcf
{
    public class MonitorBehavior : IEndpointBehavior
    {
        #region IEndpointBehavior Members

        public void AddBindingParameters(
            ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(
            ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new MonitorDispatcher());
        }

        public void Validate(ServiceEndpoint endpoint)
        { }

        #endregion

        class MonitorDispatcher : IDispatchMessageInspector
        {
            #region IDispatchMessageInspector Members

            public object AfterReceiveRequest(
                ref Message request,
                IClientChannel channel,
                InstanceContext instanceContext)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    "{0:HH:mm:ss.ffff}\t{1}\n\t\t{2} ({3} bytes)\n\t\t{4}",
                    DateTime.Now, request.Headers.MessageId,
                    request.Headers.Action, request.ToString().Length, request.Headers.To);
                return null;
            }

            public void BeforeSendReply(ref Message reply, object correlationState)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    "{0:HH:mm:ss.ffff}\t{1}\n\t\t{2} ({3} bytes)",
                    DateTime.Now, reply.Headers.RelatesTo, reply.Headers.Action,
                    reply.ToString().Length);
            }

            #endregion
        }
    }
}
