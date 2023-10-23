import { Routes, Route} from "react-router-dom";
import Home from "./pages/Home";
import Auth from "./pages/Authorization";
import Login from "./pages/Login";
import Users from "./pages/Users";
import PrivateRoute from "./UI/privateRoute/privateRoute";

function App() {
  return (
      <Routes>
        <Route path="/" element={<Home/>}/>
        <Route path="/auth" element={<Auth/>}/>
        <Route path="/login" element={<Login />} />
        <Route path="/user" element={<PrivateRoute path="/" element={<Users />}/>}>
          <Route path="listproject" element={<Users/>} />
        </Route>
      </Routes>
  );
}

export default App;
