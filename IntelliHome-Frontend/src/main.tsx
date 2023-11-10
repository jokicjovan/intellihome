
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
    {path:"/home", element: <AuthenticatedRoute><Home/>/</AuthenticatedRoute>},
    {path:"*", element: <Navigate to="/signin" replace />},
])


ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <React.StrictMode>
        <ThemeProvider theme={theme} >
            <AuthProvider>
                <RouterProvider router={router}/>
            </AuthProvider>
        </ThemeProvider>
    </React.StrictMode>
)