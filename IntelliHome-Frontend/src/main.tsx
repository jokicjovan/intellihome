import ReactDOM from 'react-dom/client'
import './index.css'
import {createBrowserRouter, Navigate, RouterProvider} from "react-router-dom";
import axios from 'axios';
import React from 'react';
import {AuthProvider} from "./security/AuthContext.tsx";
import {createTheme, ThemeProvider} from "@mui/material";
import SignIn from "./pages/SignIn.tsx";
import SignUp from "./pages/SignUp";
import Home from "./pages/Home";
import {UnauthenticatedRoute} from "./security/UnauthenticatedRoute";
import {AuthenticatedRoute} from "./security/AuthenticatedRoute";
import {QueryClient, QueryClientProvider} from "react-query";
import SuccessfulActivation from "./pages/successfulActivation";
import Layout from "./components/Shared/Layout";
import AddAdmin from "./pages/AddAdmin";
import AmbientSensorRegistrationForm from "./components/SmartDevices/Registration/PKA/AmbientSensorRegistrationForm.tsx";
import WashingMachineRegistrationForm from "./components/SmartDevices/Registration/PKA/WashingMachineRegistrationForm.tsx";

axios.defaults.withCredentials = true

const theme = createTheme({
    palette: {
        primary: {
            main: "#FBC40E",
        },
        secondary: {
            main: "#343F71",
            contrastText: 'white'
        },
    },
});

const router = createBrowserRouter([
    {path:"/SmartDeviceRegistration", element: <UnauthenticatedRoute><WashingMachineRegistrationForm smartHomeId="8f63caca-96ae-4a13-930a-e935c25e3a03"/></UnauthenticatedRoute>},
    {path:"/signin", element: <UnauthenticatedRoute><SignIn/></UnauthenticatedRoute>},
    {path:"/signup", element: <UnauthenticatedRoute><SignUp/></UnauthenticatedRoute>},
    {path:"/successfulActivation", element: <UnauthenticatedRoute><SuccessfulActivation/></UnauthenticatedRoute>},
    {path:"/home", element: <AuthenticatedRoute><Layout><Home/></Layout></AuthenticatedRoute>},
    {path:"/addAdmin", element: <UnauthenticatedRoute><Layout><AddAdmin/></Layout></UnauthenticatedRoute>},
    {path:"*", element: <Navigate to="/signin" replace />},
])
const queryClient = new QueryClient()

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
            <ThemeProvider theme={theme} >
                <AuthProvider>
                    <RouterProvider router={router}/>
                </AuthProvider>
            </ThemeProvider>
        </QueryClientProvider>
    </React.StrictMode>
)