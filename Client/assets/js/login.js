(function () {
  "use strict";

  const select = (el, all = false) => {
    el = el.trim()
    if (all) {
      return [...document.querySelectorAll(el)]
    } else {
      return document.querySelector(el)
    }
  }

  const on = (type, el, listener, all = false) => {
    let selectEl = select(el, all)
    if (selectEl) {
      if (all) {
        selectEl.forEach(e => e.addEventListener(type, listener))
      } else {
        selectEl.addEventListener(type, listener)
      }
    }
  }

  const onscroll = (el, listener) => {
    el.addEventListener('scroll', listener)
  }

  let navbarlinks = select('#navbar .scrollto', true)
  const navbarlinksActive = () => {
    let position = window.scrollY + 200
    navbarlinks.forEach(navbarlink => {
      if (!navbarlink.hash) return
      let section = select(navbarlink.hash)
      if (!section) return
      if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
        navbarlink.classList.add('active')
      } else {
        navbarlink.classList.remove('active')
      }
    })
  }
  window.addEventListener('load', navbarlinksActive)
  onscroll(document, navbarlinksActive)

  const scrollto = (el) => {
    let header = select('#header')
    let offset = header.offsetHeight

    let elementPos = select(el).offsetTop
    window.scrollTo({
      top: elementPos - offset,
      behavior: 'smooth'
    })
  }

  let selectHeader = select('#header')
  let selectTopbar = select('#topbar')
  if (selectHeader) {
    const headerScrolled = () => {
      if (window.scrollY > 100) {
        selectHeader.classList.add('header-scrolled')
        if (selectTopbar) {
          selectTopbar.classList.add('topbar-scrolled')
        }
      } else {
        selectHeader.classList.remove('header-scrolled')
        if (selectTopbar) {
          selectTopbar.classList.remove('topbar-scrolled')
        }
      }
    }
    window.addEventListener('load', headerScrolled)
    onscroll(document, headerScrolled)
  }

  let backtotop = select('.back-to-top')
  if (backtotop) {
    const toggleBacktotop = () => {
      if (window.scrollY > 100) {
        backtotop.classList.add('active')
      } else {
        backtotop.classList.remove('active')
      }
    }
    window.addEventListener('load', toggleBacktotop)
    onscroll(document, toggleBacktotop)
  }

  on('click', '.mobile-nav-toggle', function (e) {
    select('#navbar').classList.toggle('navbar-mobile')
    this.classList.toggle('bi-list')
    this.classList.toggle('bi-x')
  })

  on('click', '.navbar .dropdown > a', function (e) {
    if (select('#navbar').classList.contains('navbar-mobile')) {
      e.preventDefault()
      this.nextElementSibling.classList.toggle('dropdown-active')
    }
  }, true)

  on('click', '.scrollto', function (e) {
    if (select(this.hash)) {
      e.preventDefault()

      let navbar = select('#navbar')
      if (navbar.classList.contains('navbar-mobile')) {
        navbar.classList.remove('navbar-mobile')
        let navbarToggle = select('.mobile-nav-toggle')
        navbarToggle.classList.toggle('bi-list')
        navbarToggle.classList.toggle('bi-x')
      }
      scrollto(this.hash)
    }
  }, true)

  window.addEventListener('load', () => {
    if (window.location.hash) {
      if (select(window.location.hash)) {
        scrollto(window.location.hash)
      }
    }
  });

  let preloader = select('#preloader');
  if (preloader) {
    window.addEventListener('load', () => {
      preloader.remove()
    });
  }
  const glightbox = GLightbox({
    selector: '.glightbox'
  });

  const galelryLightbox = GLightbox({
    selector: '.galelry-lightbox'
  });

  new Swiper('.testimonials-slider', {
    speed: 600,
    loop: true,
    autoplay: {
      delay: 5000,
      disableOnInteraction: false
    },
    slidesPerView: 'auto',
    pagination: {
      el: '.swiper-pagination',
      type: 'bullets',
      clickable: true
    },
    breakpoints: {
      320: {
        slidesPerView: 1,
        spaceBetween: 20
      },

      1200: {
        slidesPerView: 2,
        spaceBetween: 20
      }
    }
  });

  new PureCounter();

  var isLoggedIn = false;
  const token = localStorage.getItem('token');
  if (token) {
    isLoggedIn = true;
  }
  var logoutButton = document.getElementById("logoutButton");
  if (isLoggedIn) {
    logoutButton.style.display = "inline-block";
  }

  on('click', '#logoutButton', function (e) {
    e.preventDefault();
    localStorage.removeItem("token");
    alert("Logout!");
  })


  on('click', '#moj-nalog', function (e) {
    e.preventDefault();

    var isLoggedIn = false;
    const token = localStorage.getItem('token');
    var userRole = localStorage.getItem('userRole');
    if (token) {
      isLoggedIn = true;
    }

    if (isLoggedIn) {
      if (userRole != 'veterinarian') {
        fetch('https://localhost:7222/api/User/Profile', {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        }).then(function (response) {
          if (!response.ok) {
            throw new Error('Error retrieving user profile.');
          }
          return response.json();
        }).then(function (user) {
          if (user.role === 'User') {
            window.location.href = 'userProfile.html';
          } else if (user.role === 'Admin') {
            window.location.href = 'adminProfile.html';
          }
        })
          .catch(function (error) {
            console.log(error);
            alert('Greška pri pristupu profila korisnika. Molimo Vas probajte opet.');
          });
      }
      else window.location.href = 'vetProfile.html';
    } else {
      console.log('User is not logged in');
      window.location.href = 'index.html';
    }
  });

  on('click', '#login', function (e) {
    e.preventDefault();

    var email = document.getElementById('email').value;
    var password = document.getElementById('password').value;
    var userType = document.getElementById('userType').value;

    var url = 'https://localhost:7222/api/Auth';

    if (userType === 'veterinarian') {
      url = 'https://localhost:7222/api/AuthVet';
    }

    fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        email: email,
        password: password
      })
    }).then(function (response) {
      if (response.ok) {
        window.location.href = 'index.html';
        return response.json();
      } else {
        throw new Error('Login failed!');
      }
    }).then(function (data) {
      localStorage.setItem('token', data.token);
      localStorage.setItem('userRole', userType);
      alert('Uspešano prijavljivanje!');

    }).catch(function (error) {
      console.error('Error:', error);
      alert('Neuspešno prijavljivanje!');
    });
  });
})()

