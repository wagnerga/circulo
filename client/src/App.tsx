import './App.css';
import { useRoutes } from 'react-router';
import PharmacyVisit from './pages/PharmacyVisit';

function App() {

  const app = useRoutes(
    [
      { path: '/', element: <PharmacyVisit /> }
    ] 
  );

  return app;
}

export default App;
