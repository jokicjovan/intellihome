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
    }

    public receiveSmartHomeSubscriptionResult(callback: (data: any) => void): void {
        if (this.connection) {
            this.connection.on('ReceiveSmartHomeSubscriptionResult', (data: any) => {
                callback(data);
            });
        }
    }

    public subscribeToSmartHome(smartHomeId: string): Promise<void> {
        if (this.connection) {
            return this.connection.invoke('SubscribeToSmartHome', smartHomeId);
        }
    }

    public stopConnection(): Promise<void> {
        if (this.connection) {
            return this.connection.stop()
        }
    }
}

export default SignalRSmartHomeService;
