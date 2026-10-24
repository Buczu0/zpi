import React from 'react'
import { createRoot } from 'react-dom/client'
import './styles.css'

function App() {
  return (
    <div className="page">
      <header className="header">
        <a className="brand" href="/" aria-label="WebStore — strona główna">
          <svg viewBox="0 0 32 32" aria-hidden="true"><path d="M7 11h18l-1 16H8L7 11Z" /><path d="M11 12V9a5 5 0 0 1 10 0v3" /></svg>
          WebStore<span className="brand-dot">.</span>
        </a>
        <span className="label">Strona startowa</span>
      </header>
      <main>
        <section className="welcome" aria-labelledby="welcome-title">
          <div className="intro">
            <p className="eyebrow">Dobrze Cię widzieć</p>
            <h1 id="welcome-title">Miejsce na<br />dobre zakupy.</h1>
            <p className="description">Witamy w WebStore. Przygotowujemy przestrzeń, w której znajdziesz coś dla siebie.</p>
            <div className="notice"><span className="status-dot" aria-hidden="true" /><p>Katalog produktów nie jest jeszcze dostępny.</p></div>
          </div>
          <div className="illustration" aria-hidden="true">
            <div className="circle" />
            <div className="package package-back"><span /></div>
            <div className="package package-front"><span /><i>W.</i></div>
            <div className="leaf leaf-one" /><div className="leaf leaf-two" />
            <div className="illustration-caption">Mały początek. Wiele możliwości.</div>
          </div>
        </section>
      </main>
      <footer><span>© 2026 WebStore</span><span>Z myślą o codziennych wyborach.</span></footer>
    </div>
  )
}

createRoot(document.getElementById('root')).render(<React.StrictMode><App /></React.StrictMode>)
