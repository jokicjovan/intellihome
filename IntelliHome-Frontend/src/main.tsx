
import ReactDOM from 'react-dom/client'
import './index.css'
import {createBrowserRouter, Navigate, RouterProvider} from "react-router-dom";
import axios from "axios";
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

axios.defaults.withCredentials = true

const theme = createTheme({
    palette: {
        primary: {
            main: "#0f0b0a",
        },
        secondary: {
            main: "#fdefc7",
            contrastText: 'white'
        },
    },
});

const router = createBrowserRouter([
    {path:"/signin", element: <UnauthenticatedRoute><SignIn/></UnauthenticatedRoute>},
    {path:"/signup", element: <UnauthenticatedRoute><SignUp/></UnauthenticatedRoute>},
    {path:"/successfulActivation", element: <UnauthenticatedRoute><SuccessfulActivation/></UnauthenticatedRoute>},
    {path:"/home", element: <AuthenticatedRoute><Layout><Home/></Layout></AuthenticatedRoute>},
    {path:"/addAdmin", element: <AuthenticatedRoute><Layout><AddAdmin/></Layout></AuthenticatedRoute>},
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