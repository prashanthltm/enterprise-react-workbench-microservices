import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Navigate, Route, Routes, NavLink } from 'react-router-dom';
import axios from 'axios';
import { useEffect, useMemo, useState } from 'react';
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';
import './index.css';

const api = {
  identity: axios.create({ baseURL: import.meta.env.VITE_IDENTITY_API_URL || 'http://localhost:5001/api' }),
  users: axios.create({ baseURL: import.meta.env.VITE_USER_API_URL || 'http://localhost:5002/api' }),
  dashboard: axios.create({ baseURL: import.meta.env.VITE_DASHBOARD_API_URL || 'http://localhost:5003/api' }),
  notifications: axios.create({ baseURL: import.meta.env.VITE_NOTIFICATION_API_URL || 'http://localhost:5004/api' })
};

function App() {
  const [user, setUser] = useState(JSON.parse(localStorage.getItem('user') || 'null'));
  const login = async (email, password) => {
    const { data } = await api.identity.post('/auth/login', { email, password });
    localStorage.setItem('user', JSON.stringify(data.user));
    localStorage.setItem('token', data.token);
    setUser(data.user);
  };
  const logout = () => { localStorage.clear(); setUser(null); };
  return <BrowserRouter><Routes><Route path="/login" element={<Login login={login} />} /><Route path="/*" element={user ? <Shell user={user} logout={logout} /> : <Navigate to="/login" />} /></Routes></BrowserRouter>;
}
function Login({ login }) {
  const [email, setEmail] = useState('prashanth@enterprise.com'); const [password,setPassword]=useState('Password@123'); const [err,setErr]=useState('');
  async function submit(e){e.preventDefault(); try{await login(email,password);}catch(ex){setErr(ex.response?.data?.message||ex.message)}}
  return <div className="min-h-screen grid place-items-center bg-slate-100"><form onSubmit={submit} className="bg-white rounded-2xl shadow p-8 w-96 space-y-4"><h1 className="text-2xl font-bold">Enterprise Workbench</h1>{err && <p className="text-red-600">{err}</p>}<input className="border p-3 rounded-xl w-full" value={email} onChange={e=>setEmail(e.target.value)} /><input className="border p-3 rounded-xl w-full" type="password" value={password} onChange={e=>setPassword(e.target.value)} /><button className="bg-blue-600 text-white rounded-xl p-3 w-full">Login</button></form></div>
}
function Shell({ user, logout }) { return <div className="min-h-screen bg-slate-50"><aside className="fixed w-72 h-full bg-white border-r p-5"><h2 className="font-bold text-xl">Workbench</h2><p className="text-sm text-slate-500">{user.name}</p><nav className="mt-8 space-y-2"><NavLink className="block p-3 rounded-xl hover:bg-slate-100" to="/">Dashboard</NavLink><NavLink className="block p-3 rounded-xl hover:bg-slate-100" to="/users">Users</NavLink><NavLink className="block p-3 rounded-xl hover:bg-slate-100" to="/notifications">Notifications</NavLink></nav><button onClick={logout} className="absolute bottom-5 left-5 right-5 bg-red-600 text-white rounded-xl p-3">Logout</button></aside><main className="ml-72 p-8"><Routes><Route path="/" element={<Dashboard/>}/><Route path="/users" element={<Users/>}/><Route path="/notifications" element={<Notifications/>}/></Routes></main></div> }
function Dashboard(){const [data,setData]=useState(null); useEffect(()=>{api.dashboard.get('/dashboard').then(r=>setData(r.data))},[]); const d=data||{totalUsers:0,activeUsers:0,notificationCount:0,teamScores:[],statusSummary:[]}; return <div className="space-y-6"><h1 className="text-3xl font-bold">Dashboard</h1><div className="grid grid-cols-3 gap-4">{[['Total Users',d.totalUsers],['Active Users',d.activeUsers],['Notifications',d.notificationCount]].map(x=><div className="bg-white rounded-2xl p-5 shadow"><p>{x[0]}</p><b className="text-3xl">{x[1]}</b></div>)}</div><div className="grid grid-cols-2 gap-4"><Chart title="Average Score"><BarChart data={d.teamScores}><XAxis dataKey="team"/><YAxis/><Tooltip/><Bar dataKey="avgScore" fill="#2563eb"/></BarChart></Chart><Chart title="Status"><PieChart><Pie data={d.statusSummary} dataKey="value" nameKey="name" label>{d.statusSummary.map((x,i)=><Cell key={x.name} fill={['#16a34a','#ef4444'][i]}/>)}</Pie><Tooltip/></PieChart></Chart></div></div>}
function Chart({title,children}){return <section className="bg-white rounded-2xl p-5 shadow"><h2 className="font-semibold mb-3">{title}</h2><ResponsiveContainer width="100%" height={260}>{children}</ResponsiveContainer></section>}
function Users(){const [users,setUsers]=useState([]);const [form,setForm]=useState({name:'',role:'',email:'',team:'',status:'Active',score:80}); const load=()=>api.users.get('/users').then(r=>setUsers(r.data)); useEffect(()=>{load()},[]); async function add(e){e.preventDefault(); await api.users.post('/users',{...form,score:Number(form.score)}); setForm({name:'',role:'',email:'',team:'',status:'Active',score:80}); load();} async function del(id){await api.users.delete(`/users/${id}`);load();} return <div className="space-y-6"><h1 className="text-3xl font-bold">Users</h1><form onSubmit={add} className="bg-white rounded-2xl p-5 shadow grid grid-cols-6 gap-3">{['name','role','email','team','score'].map(k=><input key={k} className="border rounded-xl p-3" placeholder={k} value={form[k]} onChange={e=>setForm({...form,[k]:e.target.value})}/>) }<button className="bg-blue-600 text-white rounded-xl">Add</button></form><table className="bg-white rounded-2xl shadow w-full"><tbody>{users.map(u=><tr key={u.id} className="border-b"><td className="p-3">{u.name}<div className="text-xs text-slate-500">{u.email}</div></td><td>{u.role}</td><td>{u.team}</td><td>{u.status}</td><td>{u.score}</td><td><button className="text-red-600" onClick={()=>del(u.id)}>Delete</button></td></tr>)}</tbody></table></div>}
function Notifications(){const [items,setItems]=useState([]);useEffect(()=>{api.notifications.get('/notifications').then(r=>setItems(r.data))},[]);return <div><h1 className="text-3xl font-bold mb-6">Notifications</h1>{items.map(n=><div key={n.id} className="bg-blue-50 text-blue-800 rounded-xl p-3 mb-2">{n.message}</div>)}</div>}
ReactDOM.createRoot(document.getElementById('root')).render(<App/>);
