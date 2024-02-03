import {HubConnectionBuilder, HubConnection, JsonHubProtocol} from '@microsoft/signalr';
import { environment } from '../../utils/Environment.ts';
import {HubConnectionState} from "@microsoft/signalr/src/HubConnection.ts";

class SignalRSmartDeviceService {
    private connection: HubConnection | null = null;

    constructor() {
    }

    public startConnection(): Promise<void> {
        const connectionUrl = `${environment}/hub/SmartDeviceHub`;

        this.connection = new HubConnectionBuilder()
            .withUrl(connectionUrl, { withCredentials: true })
            .withAutomaticReconnect()
            .build();

        return this.connection.start()
    }

    public receiveSmartDeviceData(callback: (data: any) => void): void {
        if (this.connection && this.connection.state == HubConnectionState.Connected) {
            this.connection.on('ReceiveSmartDeviceData', (data: any) => {
                callback(data);
            });
        }
    }

    public receiveSmartDeviceSubscriptionResult(callback: (data: any) => void): void {
        if (this.connection && this.connection.state == HubConnectionState.Connected) {
            this.connection.on('ReceiveSmartDeviceSubscriptionResult', (data: any) => {
                callback(data);
            });
        }
    }

    public subscribeToSmartDevice(smartDeviceId: string): Promise<void> {
        if (this.connection && this.connection.state == HubConnectionState.Connected) {
            return this.connection.invoke('SubscribeToSmartDevice', smartDeviceId);
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

export default SignalRSmartDeviceService;
