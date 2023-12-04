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

  var userID;

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
          throw new Error('Failed to retrieve user profile.');
        }
      })
      .then(function (user) {
        userID = user.id;
        document.getElementById('name').textContent = user.firstName + ' ' + user.lastName;
        document.getElementById('nameForm').textContent = user.firstName + ' ' + user.lastName;
        document.getElementById('email').textContent = user.email;

        fetch("https://localhost:7222/userpets?userId=" + user.id, {
          method: "GET",
          headers: {
            'Authorization': `Bearer ${token}`
          }
        }).then(response => response.json())
          .then(data => {
            const petsContainer = document.getElementById('petsContainer');
            petsContainer.innerHTML = '';

            data.forEach(pet => {
              const card = document.createElement('div');
              card.className = 'card mb-4';
              const cardBody = document.createElement('div');
              cardBody.className = 'card-body';
              const name = document.createElement('p');
              const age = document.createElement('p');
              const species = document.createElement('p');

              const zakaziDugme = document.createElement('button');
              zakaziDugme.className = "btn btn-primary mb-1";
              zakaziDugme.textContent = 'Zakaži pregled';
              zakaziDugme.style.backgroundColor = 'transparent';
              zakaziDugme.style.border = 'none';
              zakaziDugme.style.color = '#007bff';
              zakaziDugme.style.textDecoration = 'underline';
              zakaziDugme.style.cursor = 'pointer';
              const istorijaLecenja = document.createElement('button');
              istorijaLecenja.className = "btn btn-primary mb-1";
              istorijaLecenja.textContent = 'Istorija lečenja';
              istorijaLecenja.style.backgroundColor = 'transparent';
              istorijaLecenja.style.border = 'none';
              istorijaLecenja.style.color = '#007bff';
              istorijaLecenja.style.textDecoration = 'underline';
              istorijaLecenja.style.cursor = 'pointer';

              name.textContent = 'Ime: ' + pet.name;
              age.textContent = 'Starost(godine): ' + pet.age;
              species.textContent = 'Vrsta: ' + pet.species;

              cardBody.appendChild(name);
              cardBody.appendChild(age);
              cardBody.appendChild(species);
              cardBody.appendChild(zakaziDugme);
              cardBody.appendChild(istorijaLecenja);


              zakaziDugme.addEventListener("click", () => {
                window.location.href = 'zakazivanjePregleda.html?petID=' + pet.id + '&userID=' + user.id;
              });

              const includeAppointments = true;
              fetch(`https://localhost:7222/api/Pet/${pet.id}?includeAppointments=${includeAppointments}`, {
                headers: {
                  'Authorization': `Bearer ${token}`
                }
              })
                .then(response => response.json())

                .then(data => {
                  const historyContainer = document.createElement('div');
                  historyContainer.className = 'history-container';
                  historyContainer.innerHTML = '';
                  historyContainer.style.display = 'none';
                  if (data.appointments && data.appointments.length > 0) {
                    data.appointments.forEach(appointment => {

                      const appointmentDiv = document.createElement("div");
                      appointmentDiv.className = 'card mb-4';
                      const vetNameElement = document.createElement('p');
                      fetch('https://localhost:7222/api/Veterinarian/' + appointment.veterinarianId, {
                        headers: {
                          'Authorization': `Bearer ${token}`
                        }
                      }).then(response => response.json())
                        .then(data => {

                          vetNameElement.textContent = 'Ime veterinara: ' + data.firstName + ' ' + data.lastName;

                        }).catch(error => {
                          console.error('Error:', error);
                        })
                        .catch(function (error) {
                          console.error('Error:', error);
                          alert('An error occurred while retrieving vet profile.');
                        });
                      const dateElement = document.createElement("p");
                      const date = new Date(appointment.date);

                      const year = date.getFullYear();
                      const month = date.getMonth() + 1;
                      const day = date.getDate();
                      const hours = date.getHours();
                      const minutes = date.getMinutes();

                      const formattedDate = `${day}/${month}/${year}`;
                      const formattedTime = `${hours}:${minutes}`;

                      dateElement.textContent = "Datum: " + formattedDate + ' ' + formattedTime;

                      const symptomElement = document.createElement("p");
                      symptomElement.textContent = "Simptom: " + appointment.symptom;

                      const izmeniTermin = document.createElement('button');
                      izmeniTermin.textContent = 'Izmeni termin';
                      izmeniTermin.className = 'btn btn-primary mb-3';
                      izmeniTermin.style.backgroundColor = 'transparent';
                      izmeniTermin.style.color = '#24569c';
                      izmeniTermin.style.cursor = 'pointer';
                      izmeniTermin.style.width = "40%";



                      izmeniTermin.addEventListener("click", () => {
                        calendarContainer.style.display = (calendarContainer.style.display === 'none') ? 'block' : 'none';

                      });

                      const calendarContainer = document.createElement("div");
                      calendarContainer.className = "calendar-container";
                      calendarContainer.style.display = "none";
                      const calendarInfoContainer = document.createElement("div");
                      calendarInfoContainer.className = 'calendar-info-container';
                      calendarInfoContainer.innerHTML = '';

                      const datePicker = document.createElement("input");
                      datePicker.type = "datetime-local";
                      datePicker.id = "datePicker";

                      const timePicker = document.createElement("input");
                      timePicker.type = "time";
                      timePicker.id = "timePicker";

                      const symptomTxt = document.createElement("input");
                      symptomTxt.type = "text";
                      symptomTxt.id = "symptomID";

                      const submitButton = document.createElement("button");
                      submitButton.textContent = "Potvrdi izmenu";
                      submitButton.style.border = 'none';
                      submitButton.style.color = '#24569c';
                      submitButton.style.cursor = 'pointer';
                      submitButton.style.backgroundColor = 'transparent';
                      submitButton.addEventListener("click", () => {
                        const selectedDate = datePicker.value;
                        const selectedSymptom = symptomTxt.value;

                        fetch("https://localhost:7222/api/Appointment/" + appointment.id, {
                          method: "PUT",
                          headers: {
                            "Content-Type": "application/json",
                            'Authorization': `Bearer ${token}`
                          },
                          body: JSON.stringify({
                            "veterinarianId": appointment.veterinarianId,
                            "petId": appointment.petId,
                            "date": selectedDate,
                            "symptom": selectedSymptom,
                            "id": appointment.id
                          })
                        }).then(p => {
                          if (p.ok) {
                            alert("Uspesno ste izmeni termin");
                          }
                          else {
                            alert("Greska prilikom izmene termina");
                          }
                        })

                      });

                      const datePickerElement = document.createElement('p');
                      const symptomTxtElement = document.createElement('p');
                      datePickerElement.textContent = "Datum: ";
                      symptomTxtElement.textContent = "Simptom: ";

                      calendarInfoContainer.classList.add('info-container');

                      const dateContainer = document.createElement("div");
                      dateContainer.classList.add('row-container');
                      const symptomContainer = document.createElement("div");
                      symptomContainer.classList.add('row-container');
                      const submitContainer = document.createElement("div");
                      submitContainer.classList.add('row-container');
                      dateContainer.style.display = 'flex';
                      dateContainer.style.flexDirection = 'row';
                      symptomContainer.style.display = 'flex';
                      symptomContainer.style.flexDirection = 'row';
                      submitContainer.style.display = 'flex';
                      submitContainer.style.flexDirection = 'row';
                      datePicker.style.height = "60%";
                      symptomTxt.style.height = "60%";
                      dateContainer.appendChild(datePickerElement);
                      dateContainer.appendChild(datePicker);
                      symptomContainer.appendChild(symptomTxtElement);
                      symptomContainer.appendChild(symptomTxt);
                      submitContainer.appendChild(submitButton);
                      calendarInfoContainer.appendChild(dateContainer);
                      calendarInfoContainer.appendChild(symptomContainer);
                      calendarInfoContainer.appendChild(submitContainer);
                      calendarContainer.className = 'card mb-3';


                      calendarContainer.appendChild(calendarInfoContainer);
                      const obrisiTemin = document.createElement('button');
                      obrisiTemin.textContent = 'Obrisi termin';
                      obrisiTemin.className = 'btn btn-primary mb-3';
                      obrisiTemin.style.backgroundColor = 'transparent';
                      obrisiTemin.style.color = '#24569c';
                      obrisiTemin.style.cursor = 'pointer';
                      obrisiTemin.style.width = "40%";

                      obrisiTemin.addEventListener("click", function () {
                        fetch("https://localhost:7222/api/Appointment/" + appointment.id, {
                          method: "DELETE",
                          headers: {
                            'Authorization': `Bearer ${token}`
                          }
                        });
                        window.location.href = 'userProfile.html';
                      });

                      appointmentDiv.appendChild(vetNameElement);
                      appointmentDiv.appendChild(dateElement);
                      appointmentDiv.appendChild(symptomElement);
                      appointmentDiv.appendChild(izmeniTermin);
                      appointmentDiv.appendChild(calendarContainer);
                      appointmentDiv.appendChild(obrisiTemin);
                      historyContainer.appendChild(appointmentDiv);

                    });
                  }

                  cardBody.appendChild(historyContainer);
                  card.appendChild(cardBody);
                  petsContainer.appendChild(card);

                  istorijaLecenja.addEventListener("click", () => {
                    if (historyContainer.childElementCount === 0) {
                      alert('Korisnik nema istoriju lečenja!');
                    }
                    else
                      historyContainer.style.display = (historyContainer.style.display === 'none') ? 'block' : 'none';
                  });

                });
            });

          })
          .catch(error => {
            console.error('Error:', error);
          })
          .catch(function (error) {
            console.error('Error:', error);
            alert('An error occurred while retrieving user profile.');
          });

        on('click', '#togglePets', function () {
          const petsContainer = select('#petsContainer');
          petsContainer.style.display = (petsContainer.style.display === 'none') ? 'block' : 'none';
        });

        on('click', '#toggleAddPets', function () {
          window.location.href = 'dodavanjeLjubimca.html?userID=' + userID;
        });


      })
  });
})()
