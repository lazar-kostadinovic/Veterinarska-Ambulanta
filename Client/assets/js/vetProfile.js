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

    var vetID;
    var ambulanceID;
    fetch('https://localhost:7222/api/Veterinarian/profile', {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    })
      .then(function (response) {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error('Failed to retrieve vet profile.');
        }
      })
      .then(function (vet) {

        vetID = vet.id;
        ambulanceID = vet.ambulanceId;
        document.getElementById('name').textContent = vet.firstName + ' ' + vet.lastName;
        document.getElementById('nameForm').textContent = vet.firstName + ' ' + vet.lastName;
        console.log(vet.firstName);
        document.getElementById('email').textContent = vet.email;

        fetch("https://localhost:7222/api/Ambulance/" + ambulanceID, {
          method: "GET"
        })
          .then(response => response.json())
          .then(data => {
            document.getElementById('ambulanceName').textContent = data.name;
            document.getElementById('ambulanceNameForm').textContent = data.name;
          })
          .catch(error => {
            console.error('Error:', error);
          })
          .catch(function (error) {
            console.error('Error:', error);
            alert('An error occurred while retrieving veterinarian profile.');
          });


        function fetchAppointments() {
          fetch("https://localhost:7222/api/Appointment/veterinarian/" + vetID, {
            method: "GET"
          })
            .then(response => response.json())
            .then(data => {
              const appointmentContainer = document.getElementById('appointmentsContainer');
              appointmentContainer.innerHTML = '';
              data.forEach(appointment => {
                const card = document.createElement('div');
                card.className = 'card mb-4';
                const cardBody = document.createElement('div');
                cardBody.className = 'card-body';
                const appointmentDate = document.createElement('p');
                const appointmentPet = document.createElement('p');
                const appointmentSymptom = document.createElement('p');

                const petButton = document.createElement('button');
                petButton.textContent = 'Više informacija o ljubimcu';
                petButton.id = 'pet';
                petButton.className = 'btn btn-primary mb-3';

                const petInfoContainer = document.createElement('div');
                petInfoContainer.className = 'pet-info-container';
                petInfoContainer.style.display = 'none';

                const date = new Date(appointment.date);

                const year = date.getFullYear();
                const month = date.getMonth() + 1;
                const day = date.getDate();
                const hours = date.getHours();
                const minutes = date.getMinutes();

                const formattedDate = `${day}/${month}/${year}`;
                const formattedTime = `${hours}:${minutes}`;

                var petName;
                fetch("https://localhost:7222/api/Pet/" + appointment.petId, {
                  headers: {
                    'Authorization': `Bearer ${token}`
                  }
                })
                  .then(response => response.json())
                  .then(d => {
                    petName = d.name;
                    appointmentDate.textContent = `Datum i vreme: ${formattedDate} ${formattedTime}`;
                    appointmentPet.textContent = `Ljubimac: ${petName}`;
                    appointmentSymptom.textContent = `Simptomi: ${appointment.symptom}`;

                    petButton.addEventListener('click', function () {
                      petInfoContainer.style.display = (petInfoContainer.style.display === 'none') ? 'block' : 'none';
                    });

                    fetch("https://localhost:7222/api/Pet/" + appointment.petId, {
                      headers: {
                        'Authorization': `Bearer ${token}`
                      }
                    })
                      .then(response => response.json())
                      .then(dt => {


                        const petName = document.createElement('div');
                        petName.className = 'pet-name';
                        petName.textContent = `Ime: ${dt.name}`;

                        const petAge = document.createElement('div');
                        petAge.className = 'pet-age';
                        petAge.textContent = `Starost: ${dt.age}`;

                        const petSpecies = document.createElement('div');
                        petSpecies.className = 'pet-species';
                        petSpecies.textContent = `Vrsta: ${dt.species}`;

                        const ownerInfo = document.createElement('div');
                        ownerInfo.className = 'owner-info';

                        fetch("https://localhost:7222/api/User/" + dt.userId, {
                          headers: {
                            'Authorization': `Bearer ${token}`
                          }
                        })
                          .then(response => response.json())
                          .then(p => {
                            const ownerName = `${p.firstName} ${p.lastName}`;
                            const ownerEmail = p.email;

                            ownerInfo.innerHTML = `Vlasnik: <span class="owner-name">${ownerName}</span> (${ownerEmail})`;

                            petInfoContainer.appendChild(petName);
                            petInfoContainer.appendChild(petAge);
                            petInfoContainer.appendChild(petSpecies);
                            petInfoContainer.appendChild(ownerInfo);

                          })
                          .catch(error => console.error('Error fetching owner informarion:', error));

                      })
                      .catch(error => console.error('Error fetching pet information:', error));
                  }
                  )
                  .catch(error => console.error('Error fetching pet name:', error));
                cardBody.appendChild(appointmentDate);
                cardBody.appendChild(appointmentPet);
                cardBody.appendChild(appointmentSymptom);
                cardBody.appendChild(petButton);
                cardBody.appendChild(petInfoContainer);

                card.appendChild(cardBody);
                appointmentContainer.appendChild(card);
              });
            })
            .catch(error => console.error('Error fetching appointments:', error));
        }



        function fetchReviews() {
          fetch("https://localhost:7222/api/Review/veterinarian/" + vetID, {
            headers: {
              'Authorization': `Bearer ${token}`
            }
          })
            .then(response => response.json())
            .then(data => {
              const commentsContainer = document.getElementById('commentsContainer');
              commentsContainer.innerHTML = '';
              data.forEach(review => {
                const commentDiv = document.createElement('div');
                commentDiv.classList.add('card', 'mb-4');

                const commentCardBody = document.createElement('div');
                commentCardBody.classList.add('card-body');

                const commentText = document.createElement('p');
                commentText.textContent = review.comment;

                const commentAuthor = document.createElement('div');
                commentAuthor.classList.add('d-flex', 'flex-row', 'align-items-center');

                const commentAuthorImage = document.createElement('img');
                commentAuthorImage.src = "assets/img/user.jpg"
                commentAuthorImage.alt = 'avatar';
                commentAuthorImage.width = 25;
                commentAuthorImage.height = 25;

                const commentAuthorName = document.createElement('p');
                commentAuthorName.classList.add('small', 'mb-0', 'ms-2');


                fetch("https://localhost:7222/api/User/" + review.userId, {
                  headers: {
                    'Authorization': `Bearer ${token}`
                  }
                })
                  .then(response => response.json())
                  .then(data => {
                    commentAuthorName.textContent = data.firstName + ' ' + data.lastName;
                  })
                  .catch(error => console.error('Error fetching comentator informarion:', error));

                commentAuthor.appendChild(commentAuthorImage);
                commentAuthor.appendChild(commentAuthorName);

                commentCardBody.appendChild(commentText);
                commentCardBody.appendChild(commentAuthor);

                commentDiv.appendChild(commentCardBody);

                commentsContainer.appendChild(commentDiv);
              });
            })
            .catch(error => console.error('Error fetching reviews:', error));
        }

        on('click', '#toggleAppointments', function () {
          const appointmentsContainer = select('#appointmentsContainer');
          appointmentsContainer.style.display = (appointmentsContainer.style.display === 'none') ? 'block' : 'none';
          if (appointmentsContainer.style.display === 'block') {
            fetchAppointments();
          }
        });

        on('click', '#toggleReviews', function () {
          const reviewsContainer = select('#commentsContainer');
          reviewsContainer.style.display = (reviewsContainer.style.display === 'none') ? 'block' : 'none';
          if (reviewsContainer.style.display === 'block') {
            fetchReviews();
          }
        });
      })
  });
})()
