import './App.css';
import Search from './containers/Search/Search';
import Layout from './hoc/Layout/Layout';

function App() {
  return (
    <div className="App">
      <Layout>
        <Search/>
      </Layout>
    </div>
  );
}

export default App;
