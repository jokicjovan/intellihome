import React, {useState} from "react";
import {MapContainer, Popup, Marker, TileLayer, useMapEvents} from "react-leaflet";
import L from "leaflet";


const SmartHomeCreatingMap = () => {
    const position = L.latLng(45.2671, 19.8335);

    const [clickedMarker, setClickedMarker] = useState(null);

    const handleClick = async (event) => {
        const { lat, lng } = event.latlng;
        console.log(`Clicked at latitude ${lat}, longitude ${lng}`);

        try {
            const response = await fetch(`https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lng}`);
            const data = await response.json();

            if (data && data.address) {
                const { address } = data;
                console.log('Address found:', address);

                console.log(`Address: ${address.road + ' ' + address.house_number}`);
                console.log(`City: ${address.city_district}`);
                console.log(`Country: ${address.country}`);

                // Set the clicked marker for display on the map
                setClickedMarker({
                    position: [lat, lng],
                });
            } else {
                console.log('Address not found');
            }
        } catch (error) {
            console.error('Error fetching address:', error);
        }
    };

    return (
        <MapContainer center={position} zoom={13} style={{ height: '100%', width: '100%' }}>
            <TileLayer
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            />

            {/* Display the clicked marker on the map */}
            {clickedMarker && (
                <Marker position={clickedMarker.position}>
                    <Popup>{clickedMarker.popupContent}</Popup>
                </Marker>
            )}

            <ClickHandler handleClick={handleClick} />
        </MapContainer>
    );
};

// Custom component to handle map click events
const ClickHandler = ({ handleClick }) => {
    const map = useMapEvents({
        click: (event) => {
            handleClick(event);
        },
    });

    return null; // No additional elements to render
};

export default SmartHomeCreatingMap;