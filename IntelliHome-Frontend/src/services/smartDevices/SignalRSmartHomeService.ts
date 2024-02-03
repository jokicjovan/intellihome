import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { environment } from '../../utils/Environment.ts';
import {HubConnectionState} from "@microsoft/signalr/src/HubConnection.ts";

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
        if (this.connection && this.connection.state == HubConnectionState.Connected) {
            this.connection.on('ReceiveSmartHomeSubscriptionResult', (data: any) => {
                callback(data);
            });
        }
    }
    public receiveSmartHomeData(callback: (data: any) => void): void {
        if (this.connection && this.connection.state == HubConnectionState.Connected) {
            this.connection.on('ReceiveSmartHomeData', (data: any) => {
                callback(data);
            });
        }
    }

    public subscribeToSmartHome(smartHomeId: string): Promise<void> {
        if (this.connection && this.connection.state == HubConnectionState.Connected) {
            return this.connection.invoke('SubscribeToSmartHome', smartHomeId);
        }
    }

    public stopConnection(): Promise<void> {
        if (this.connection) {
            return this.connection.stop()
                .catch((error) => {
                    console.error('Error stopping SignalR connection: ', error);
                });
        }
        return Promise.resolve();
    }
}

export default SignalRSmartHomeService;
