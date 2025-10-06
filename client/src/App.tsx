import { createBrowserRouter,type RouteObject, RouterProvider } from "react-router-dom"
import './App.css'


import Home from "./components/Home.tsx";
import Books from "./components/Books.tsx";
import Authors from "./components/Authors.tsx";
import Genre from "./components/Genre.tsx";



const myRoutes : RouteObject[] =[
    {
        path: '/',
        element: <Home/>
    },
    {
        path: '/books',
        element: <Books/>
    },
    {
        path: '/authors',
        element: <Authors/>
    },
    {
        path: '/genre',
        element: <Genre/>
    }
]

function App() {
  return <RouterProvider router = {createBrowserRouter(myRoutes)} />
}

export default App
