﻿/*
    Copyright (C) 2015  Jack Fagner

    This file is part of OpenTidl.

    OpenTidl is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    OpenTidl is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with OpenTidl.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTidl.Models;
using OpenTidl.Models.Base;
using OpenTidl.Transport;
using OpenTidl.Enums;

namespace OpenTidl
{
    public partial class OpenTidlClient
    {

        #region album methods

        public Task<AlbumModel> GetAlbumAsync(Int32 albumId)
        {
            return RestClient.HandleAsync<AlbumModel>(
                RestUtility.FormatUrl("/albums/{id}", new { id = albumId }), new
                {
                    token = Configuration.Token,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<ModelArray<AlbumModel>> GetAlbumsAsync(IEnumerable<Int32> albumIds)
        {
            return RestClient.HandleAsync<ModelArray<AlbumModel>>(
                "/albums", new
                {
                    ids = String.Join(",", albumIds),
                    token = Configuration.Token,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<AlbumModel>> GetSimilarAlbumsAsync(Int32 albumId)
        {
            return RestClient.HandleAsync<JsonList<AlbumModel>>(
                RestUtility.FormatUrl("/albums/{id}/similar", new { id = albumId }), new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> GetAlbumTracksAsync(Int32 albumId)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                RestUtility.FormatUrl("/albums/{id}/tracks", new { id = albumId }), new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<CreditLink>> GetAlbumTracksWithCreditsAsync(Int32 albumId, int offset = 0, int limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<CreditLink>>(
                RestUtility.FormatUrl("/albums/{id}/items/credits", new { id = albumId }), new
                {
                    replace = true,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<AlbumReviewModel> GetAlbumReviewAsync(Int32 albumId)
        {
            return RestClient.HandleAsync<AlbumReviewModel>(
                RestUtility.FormatUrl("/albums/{id}/review", new { id = albumId }), new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        #endregion


        #region artist methods

        public Task<ArtistModel> GetArtistAsync(Int32 artistId)
        {
            return RestClient.HandleAsync<ArtistModel>(
                RestUtility.FormatUrl("/artists/{id}", new { id = artistId }), new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<AlbumModel>> GetArtistAlbumsAsync(
            int artistId, AlbumFilter filter, int offset = 0, int limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<AlbumModel>>(
                RestUtility.FormatUrl("/artists/{id}/albums", new { id = artistId }), new
                {
                    filter = filter.ToString("F"),
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> GetRadioFromArtistAsync(Int32 artistId, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                RestUtility.FormatUrl("/artists/{id}/radio", new { id = artistId }), new
                {
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<ArtistModel>> GetSimilarArtistsAsync(Int32 artistId, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<ArtistModel>>(
                RestUtility.FormatUrl("/artists/{id}/similar", new { id = artistId }), new
                {
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> GetArtistTopTracksAsync(Int32 artistId, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                RestUtility.FormatUrl("/artists/{id}/toptracks", new { id = artistId }), new
                {
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<VideoModel>> GetArtistVideosAsync(Int32 artistId, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<VideoModel>>(
                RestUtility.FormatUrl("/artists/{id}/videos", new { id = artistId }), new
                {
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<ArtistBiographyModel> GetArtistBiographyAsync(Int32 artistId)
        {
            return RestClient.HandleAsync<ArtistBiographyModel>(
                RestUtility.FormatUrl("/artists/{id}/bio", new { id = artistId }), new
                {
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<LinkModel>> GetArtistLinksAsync(Int32 artistId, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<LinkModel>>(
                RestUtility.FormatUrl("/artists/{id}/links", new { id = artistId }), new
                {
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        #endregion


        #region country methods

        public Task<CountryModel> GetCountryAsync()
        {
            return RestClient.HandleAsync<CountryModel>("/country", null, null, "GET");
        }

        #endregion

        
        #region search methods

        public Task<JsonList<AlbumModel>> SearchAlbumsAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<AlbumModel>>(
                "/search/albums", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<ArtistModel>> SearchArtistsAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<ArtistModel>>(
                "/search/artists", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<PlaylistModel>> SearchPlaylistsAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<PlaylistModel>>(
                "/search/playlists", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> SearchTracksAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                "/search/tracks", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<VideoModel>> SearchVideosAsync(String query, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<VideoModel>>(
                "/search/videos", new
                {
                    query,
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<SearchResultModel> SearchAsync(String query, SearchType types, Int32 offset = 0, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<SearchResultModel>(
                "/search", new
                {
                    query,
                    types = types.ToString(),
                    offset,
                    limit,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        #endregion


        #region track methods

        public Task<TrackModel> GetTrackAsync(Int32 trackId)
        {
            return RestClient.HandleAsync<TrackModel>(
                RestUtility.FormatUrl("/tracks/{id}", new { id = trackId }), new
                {
                    token = Configuration.Token,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<ContributorModel>> GetTrackContributorsAsync(Int32 trackId)
        {
            return RestClient.HandleAsync<JsonList<ContributorModel>>(
                RestUtility.FormatUrl("/tracks/{id}/contributors", new { id = trackId }), new
                {
                    token = Configuration.Token,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        public Task<JsonList<TrackModel>> GetRadioFromTrackAsync(Int32 trackId, Int32 limit = OpenTidlConstants.DEFAULT_LIMIT)
        {
            return RestClient.HandleAsync<JsonList<TrackModel>>(
                RestUtility.FormatUrl("/tracks/{id}/radio", new { id = trackId }), new
                {
                    limit,
                    token = Configuration.Token,
                    countryCode = GetCountryCode()
                }, null, "GET");
        }

        #endregion
    }
}
