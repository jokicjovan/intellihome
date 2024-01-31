import ReactDOM from 'react-dom/client'
import './index.css'
import {createBrowserRouter, Navigate, RouterProvider} from "react-router-dom";
import axios from 'axios';
import React from 'react';
import {AuthProvider} from "./security/AuthContext.tsx";
import {createTheme, ThemeProvider} from "@mui/material";
import SignInPage from "./pages/SignInPage.tsx";
import SignUpPage from "./pages/SignUpPage.tsx";
import HomePage from "./pages/HomePage.tsx";
import {UnauthenticatedRoute} from "./security/UnauthenticatedRoute";
import {AuthenticatedRoute} from "./security/AuthenticatedRoute";
import {QueryClient, QueryClientProvider} from "react-query";
import SuccessfulActivationPage from "./pages/SuccessfulActivationPage.tsx";
import Layout from "./components/Shared/Layout";
import AddAdminPage from "./pages/AddAdminPage.tsx";
import LandingPage from "./pages/LandingPage";
import {AdminRoute} from "./security/AdminRoute";
import PasswordChangePage from "./pages/PasswordChangePage.tsx";
import {AdminFirstTimeRoute} from "./security/AdminFirstTimeRoute";
import SmartHomePage from "./pages/SmartHomePage.tsx";
import SmartDevicePage from "./pages/SmartDevicePage.tsx";

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
    {path:"/signin", element: <UnauthenticatedRoute><SignInPage/></UnauthenticatedRoute>},
    {path:"/signup", element: <UnauthenticatedRoute><SignUpPage/></UnauthenticatedRoute>},
    {path:"/successfulActivation", element: <UnauthenticatedRoute><SuccessfulActivationPage/></UnauthenticatedRoute>},
    {path:"/index", element: <UnauthenticatedRoute><LandingPage/></UnauthenticatedRoute>},
    {path:"/home", element: <AuthenticatedRoute><Layout><HomePage/></Layout></AuthenticatedRoute>},
    {path:"/consumption", element: <AuthenticatedRoute><AdminRoute></AdminRoute></AuthenticatedRoute>},
    {path:"/addAdmin", element: <AuthenticatedRoute><AdminRoute><Layout><AddAdminPage/></Layout></AdminRoute></AuthenticatedRoute>},
    {path:"/passwordChange", element: <AuthenticatedRoute><AdminFirstTimeRoute><PasswordChangePage/></AdminFirstTimeRoute></AuthenticatedRoute>},
    {path:"/smartHome/:id", element: <AuthenticatedRoute><Layout><SmartHomePage/></Layout></AuthenticatedRoute>},
    {path:"/smartDevice/:type/:id", element: <AuthenticatedRoute><Layout><SmartDevicePage/></Layout></AuthenticatedRoute>},
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