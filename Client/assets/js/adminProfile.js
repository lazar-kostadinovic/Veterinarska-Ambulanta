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
    window.location.href = 'index.html';
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
            alert('Greška prilikom pristupu profila korsnika. Molimo Vas probajte ponovo!');
          });
      }
      else window.location.href = 'vetProfile.html';
    } else {
      window.location.href = 'login.html';
    }
  });

  window.addEventListener('load', () => {
    fetch('https://localhost:7222/api/User/Profile', {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    })
      .then(function (response) {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error('Failed to retrieve admin profile.');
        }
      })
      .then(function (admin) {
        document.getElementById('name').textContent = admin.firstName + ' ' + admin.lastName;
        document.getElementById('nameForm').textContent = admin.firstName + ' ' + admin.lastName;
        document.getElementById('email').textContent = admin.email;

        function fetchUsers() {
          fetch("https://localhost:7222/api/User", {
            headers: {
              'Authorization': `Bearer ${token}`
            },
            method: "GET"
          })
            .then(response => response.json())
            .then(data => {
              const userContainer = document.getElementById('usersContainer');
              userContainer.innerHTML = '';
              console.log(data);
              data.forEach(user => {
                console.log(user);
                const card = document.createElement('div');
                card.className = 'card mb-4';
                const cardBody = document.createElement('div');
                cardBody.className = 'card-body';
                const id = document.createElement('p');
                const name = document.createElement('p');
                const email = document.createElement('p');

                const deleteButton = document.createElement('button');
                deleteButton.textContent = 'Obriši korisnika';
                deleteButton.id = 'delete';
                deleteButton.className = 'btn btn-primary mb-3';
                deleteButton.style.backgroundColor = 'transparent';
                deleteButton.style.color = '#24569c';
                deleteButton.style.cursor = 'pointer';

                const getPetsButton = document.createElement('button');
                getPetsButton.textContent = 'Ljubimci';
                getPetsButton.id = 'getPets';
                getPetsButton.className = 'btn btn-primary mb-3';

                const petContainer = document.createElement('div');
                petContainer.className = 'pet-container';
                petContainer.style.display = 'none';

                id.textContent = 'Identifikator: ' + user.id;
                name.textContent = 'Ime: ' + user.firstName + ' ' + user.lastName;
                email.textContent = 'Email: ' + user.email;

                getPetsButton.addEventListener('click', function () {
                  if (petContainer.childElementCount === 0) {
                    alert('Korisnik nema ljubimaca!');
                  }
                  else
                    petContainer.style.display = (petContainer.style.display === 'none') ? 'block' : 'none';
                });


                deleteButton.addEventListener('click', function () {
                  fetch("https://localhost:7222/api/User/" + user.id, {
                    method: "DELETE",
                    headers: {
                      'Authorization': `Bearer ${token}`
                    }
                  });
                  window.location.href = 'adminProfile.html';
                });

                fetch("https://localhost:7222/userpets?userId=" + user.id, {
                  method: "GET",
                  headers: {
                    'Authorization': `Bearer ${token}`
                  }
                })
                  .then(response => response.json())
                  .then(data => {
                    const petInfoContainer = document.createElement('div');
                    petInfoContainer.className = 'pet-info-container';
                    petInfoContainer.innerHTML = '';

                    data.forEach(pet => {
                      const petDiv = document.createElement("div");
                      petDiv.className = 'card mb-4';

                      const petInfo = document.createElement("p");
                      petInfo.textContent = `${pet.name} (starosti: ${pet.age}) - ${pet.species}`;

                      const zakaziDugme = document.createElement('button');
                      zakaziDugme.className = "btn btn-primary mb-1";
                      zakaziDugme.textContent = 'Zakaži pregled';
                      zakaziDugme.style.backgroundColor = 'transparent';
                      zakaziDugme.style.border = 'none';
                      zakaziDugme.style.color = '#007bff';
                      zakaziDugme.style.textDecoration = 'underline';
                      zakaziDugme.style.cursor = 'pointer';
                      petDiv.appendChild(petInfo);
                      petInfo.appendChild(zakaziDugme);
                      petInfoContainer.appendChild(petDiv);
                      petContainer.appendChild(petInfoContainer);
                      zakaziDugme.addEventListener("click", () => {
                        window.location.href = 'zakazivanjePregledaAdmin.html?petID=' + pet.id + '&userID=' + user.id;
                      });
                    });
                  })
                  .catch(error => {
                    console.error('Error:', error);
                  });

                cardBody.appendChild(id);
                cardBody.appendChild(name);
                cardBody.appendChild(email);
                cardBody.appendChild(getPetsButton);
                cardBody.appendChild(petContainer);
                cardBody.appendChild(deleteButton);

                card.appendChild(cardBody);
                userContainer.appendChild(card);
              });
            })
            .catch(error => console.error('Error fetching users:', error));
        }

        function fetchVets() {
          fetch("https://localhost:7222/api/Veterinarian", {
            headers: {
              'Authorization': `Bearer ${token}`
            },
            method: "GET"
          })
            .then(response => response.json())
            .then(data => {
              const vetsContainer = document.getElementById('vetsContainer');
              vetsContainer.innerHTML = '';
              console.log(data);
              data.forEach(vets => {
                console.log(vets);
                const card = document.createElement('div');
                card.className = 'card mb-4';
                const cardBody = document.createElement('div');
                cardBody.className = 'card-body';
                const id = document.createElement('p');
                const name = document.createElement('p');
                const email = document.createElement('p');
                const imeAmbulante = document.createElement('p');
                imeAmbulante.id = "ambulanceName";

                const deleteVet = document.createElement('button');
                deleteVet.textContent = 'Obriši veterinara';
                deleteVet.id = 'delete';
                deleteVet.className = 'btn btn-primary mb-3';
                deleteVet.style.backgroundColor = 'transparent';
                deleteVet.style.color = '#24569c';
                deleteVet.style.cursor = 'pointer';

                deleteVet.addEventListener("click", () => {
                  obrisiVeterinara(vets.id, vets.ambulanceId);
                });
                function obrisiVeterinara(vetID, ambID) {
                  fetch(`https://localhost:7222/api/Veterinarian/${vetID}?ambulanceId=${ambID}`, {
                    method: "DELETE"
                  });
                }

                id.textContent = 'Identifikator: ' + vets.id;
                name.textContent = 'Ime: ' + vets.firstName + ' ' + vets.lastName;
                email.textContent = 'Email: ' + vets.email;


                fetch("https://localhost:7222/api/Ambulance/" + vets.ambulanceId, {
                  method: "GET"
                })
                  .then(response => response.json())
                  .then(data => {
                    imeAmbulante.textContent = 'Ime ambulante: ' + data.name;
                  })
                  .catch(error => {
                    console.error('Error:', error);
                  })
                  .catch(function (error) {
                    console.error('Error:', error);
                    alert('An error occurred while retrieving veterinarian profile.');
                  });




                cardBody.appendChild(id);
                cardBody.appendChild(name);
                cardBody.appendChild(email);
                cardBody.appendChild(imeAmbulante);
                cardBody.appendChild(deleteVet);

                card.appendChild(cardBody);
                vetsContainer.appendChild(card);
              });
            })
            .catch(error => console.error('Error fetching users:', error));
        }

        on('click', '#toggleUsers', function () {
          const usersContainer = select('#usersContainer');
          usersContainer.style.display = (usersContainer.style.display === 'none') ? 'block' : 'none';
          if (usersContainer.style.display === 'block') {
            fetchUsers();
          }
        });

        on('click', '#toggleVets', function () {
          const vetsContainer = select('#vetsContainer');
          vetsContainer.style.display = (vetsContainer.style.display === 'none') ? 'block' : 'none';
          if (vetsContainer.style.display === 'block') {
            fetchVets();
          }
        });

        on('click', '#regVet', function () {
          console.log("sss");
          window.location.href = 'vetRegister.html';
        });

      })
  });
})()
