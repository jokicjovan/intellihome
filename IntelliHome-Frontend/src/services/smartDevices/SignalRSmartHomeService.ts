import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { environment } from '../../utils/Environment.ts';

class SignalRSmartHomeService {
    private connection: HubConnection | null = null;

    constructor() {
    }

    public startConnection(): Promise<void> {
        const connectionUrl = `${environment}/hub/SmartHomeHub`;

        this.connection = new HubConnectionBuilder()
            .withUrl(connectionUrl, { withCredentials: true })
            .withAutomaticReconnect()
            .build();

        return this.connection.start()
            .then(() => {
                console.log('SignalR connection established');
            })
            .catch((error) => {
                console.error('SignalR connection error: ', error);
            });
    }

    public receiveSmartHomeSubscriptionResult(callback: (data: any) => void): void {
        if (this.connection) {
            this.connection.on('ReceiveSmartHomeSubscriptionResult', (data: any) => {
                callback(data);
            });
        }
    }

    public subscribeToSmartHome(smartHomeId: string): void {
        if (this.connection) {
            this.connection.invoke('SubscribeToSmartHome', smartHomeId);
        }
    }

    public stopConnection(): Promise<void> {
        if (this.connection) {
            return this.connection.stop()
                .then(() => {
                    console.log('SignalR connection stopped');
                })
                .catch((error) => {
                    console.error('Error stopping SignalR connection: ', error);
                });
        }

        return Promise.resolve();
    }
}

export default SignalRSmartHomeService;
